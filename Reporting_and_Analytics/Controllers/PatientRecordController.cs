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
                    type_of_service = item.type_of_service.ToString()
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
