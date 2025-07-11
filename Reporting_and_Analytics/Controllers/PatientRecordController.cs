using Microsoft.AspNetCore.Mvc;
using Models_and_Enums.Enums;
using Models_and_Enums.patient_and_treatment_statistics;
using Models_and_Enums.Request.PatientReport;
using Models_and_Enums.Responses.PatientReport;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Controllers
{
    [ApiController]
    [Route("api/PatientRecord/")]
	public class PatientRecordController : ControllerBase
	{
        private readonly DatabaseContext _databaseContext;
        private readonly IPatientRecordsRepository _patientRecordsRepository;
        public PatientRecordController(DatabaseContext databaseContext,IPatientRecordsRepository patientRecordsRepository)
        {
            _databaseContext = databaseContext;
            _patientRecordsRepository = patientRecordsRepository;
        }


        [HttpGet("first-name-search/{first_name}")]
        public async Task<IActionResult> FirstNameSearch(string first_name)
        {
            try
            {
                var results =  _databaseContext.PatientRecords.AsEnumerable()
                                                              .Where(i => i.first_name == first_name.ToLower())
                                                              .ToList();
                if(results.Count == 0 || results == null)
                {
                    return NotFound();
                }

                var response = results.Select(i => new PatientDetailsResponse
                {
                    Full_name = i.Full_name,
                    Age = i.Age,
                    admission_date = i.admission_date.ToShortDateString(),
                    Sex = i.Sex.ToString(),
                    type_of_service = i.type_of_service.ToString(),
                }).ToList();

                return Ok(response);

            }
            catch (NullReferenceException ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("last-name-search/{last_name}")]
        public async Task<IActionResult> LastNameSearch(string last_name)
        {
            try
            {
                var results = _databaseContext.PatientRecords.AsEnumerable()
                                                             .Where(i => i.last_name == last_name.ToLower())
                                                             .ToList(); 

                if(results != null && results.Count != 0) 
                {
                    var response = results.Select(i => new PatientDetailsResponse
                    {
                        Full_name=i.Full_name,
                        admission_date = i.admission_date.ToShortDateString(),
                        Age = i.Age,
                        Sex = i.Sex.ToString(),
                        type_of_service = i.type_of_service.ToString(),
                    }).OrderBy(i => i.Full_name).ToList();

                    return Ok(response);
                }

                return NotFound();
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(500,ex.Message);
            }
        }



        [HttpPost("add-new-patient")]
        public async Task<IActionResult> NewPatient([FromBody]AddNewPatientRequest newPatientRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var new_patient = new PatientRecords
                {
                    Full_name = newPatientRequest.Full_name,
                    Age = newPatientRequest.Age,
                    admission_date = DateTime.Now,
                    Sex = newPatientRequest.Sex,
                    PHIC = newPatientRequest.PHIC,
                    type_of_service = newPatientRequest.type_of_service
                };

                _databaseContext.PatientRecords.Add(new_patient);
                await _databaseContext.SaveChangesAsync();

                var response = new PatientDetailsResponse
                {
                    Full_name = new_patient.Full_name,
                    Age = new_patient.Age,
                    admission_date = new_patient.admission_date.ToShortDateString(),
                    PHIC = new_patient.PHIC,
                    Sex = Enum.GetName(typeof(Gender),new_patient.Sex),
                    type_of_service = new_patient.type_of_service.ToString()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("get-patient-records")]
        public async Task<IActionResult> GetRecords()
        {
            try
            {
                var records = await _patientRecordsRepository.GetAllPatientsRecords();

                if(records.Count() <= 0 || records == null)
                {
                    return Ok(records);
                }

                var response =  records.Select(item => new PatientDetailsResponse
                {
                    Full_name=item.Full_name,
                    Age = item.Age,
                    admission_date = item.admission_date.ToShortDateString(),
                    PHIC = item.PHIC,
                    Sex=item.Sex.ToString(),
                    type_of_service = item.type_of_service.ToString(),
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpDelete("delete-patient/{patient_id}")]
        public async Task<IActionResult> DeletePatientById(Guid patient_id)
        {
            try
            {
                var patient_to_delete = await _patientRecordsRepository.GetPatientRecordsById(patient_id);

                if(patient_to_delete ==  null) 
                {
                    return NotFound();
                }

                _databaseContext.PatientRecords.Remove(patient_to_delete);
                await _databaseContext.SaveChangesAsync();

                return Ok(patient_to_delete);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
