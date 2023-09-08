using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WeatherForecast.Controllers;

[ApiController]
[Route("api/estudiantes")]
public class EstudiantesController : ControllerBase{

    private static string[] nombres = {"Juan", "Pedro", "Pablo"};
    private static string[] apellidos = {"Ap1", "Ap2", "Ap3"};
    private static Dictionary<int,Estudiante> estudiantes = initializeEstudiantes();


    /*La lógica debería estar toda en una clase controlador*/
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultadoConsultaEstudiantes))]
    public IActionResult Get(){
             
        ResultadoConsultaEstudiantes resultado = new ResultadoConsultaEstudiantes();
        resultado.page = 1;
        resultado.size = estudiantes.Count;
        resultado.estudiantes = estudiantes.Values;
        return Ok(resultado);
    }

    [HttpGet]
    [Route("{id?}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Estudiante))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiError))]
    public IActionResult Get(int id){

        if (!estudiantes.ContainsKey(id)){
            ApiError apiError = new ApiError();
            apiError.addError("NOT_FOUND", "No existe un estudiante con el id provisto");
            apiError.statusCode = 404;
            return NotFound(apiError);
        }

        return Ok(estudiantes[id]);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Estudiante))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    public IActionResult Post(Estudiante estudiante) {        

        if (estudiante == null) {
            ApiError apiError= new ApiError();
            apiError.addError("NULL_VALUE", "No se proveyeron los datos del estudiante a registrar");
            apiError.statusCode = 400;
            return BadRequest(apiError);
        }
        Dictionary<string, string> errors = estudiante.validate_new();

        if (errors != null) {
            ApiError apiError= new ApiError();
            apiError.errors = errors;
            apiError.statusCode = 400;
            return BadRequest(apiError);
        }

        if (estudiante.id == 0) {
            estudiante.id = Random.Shared.Next(0, 2000);
        } 

        if(estudiantes.ContainsKey(estudiante.id)) {
            ApiError apiError = new ApiError();
            apiError.addError("EXISTENTE", "Ya existe un estudiante registrado con el id provisto");
            apiError.statusCode = 400;
            return BadRequest(apiError);
        }

        estudiantes.Add(estudiante.id, estudiante);

        return Ok(estudiante);
    }

    [HttpDelete]
    [Route("{id?}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Estudiante))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiError))]
    public IActionResult Delete(int id){
        
        if(!estudiantes.ContainsKey(id)){

            ApiError apiError = new ApiError();
            apiError.addError("NOT_FOUND", "No existe un estudiante con el id provisto");
            apiError.statusCode = 404;
            return NotFound(apiError);
        }
        
        Estudiante estudiante = estudiantes[id];
        estudiantes.Remove(id);
        return Ok(estudiante);        
    }

    [HttpPut]
    [Route("{id?}")]
    /*[SwaggerOperation(
        Summary = "Actualiza los datos de un estudiante",
        Description = "Retorna los datos del estudiante en su estado previo a la modificación")]*/
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Estudiante))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    public IActionResult Put(int id, Estudiante estudiante){

        if(!estudiantes.ContainsKey(id)){
            ApiError apiError = new ApiError();
            apiError.addError("NOT_FOUND", "No existe un estudiante con el id provisto");
            apiError.statusCode = 404;
            return NotFound(apiError);
        }
        if(estudiante.id != id){
            ApiError apiError = new ApiError();
            apiError.addError("NO_MATCH", "El id provisto no coincide con el estudiante provisto");
            apiError.statusCode = 400;
            return BadRequest(apiError);
        }

        Dictionary<string, string> errors = estudiante.validate_new();
        if (errors != null){
            ApiError apiError = new ApiError();
            apiError.errors = errors;
            apiError.statusCode = 400;
            return BadRequest(apiError);
        }
        
        Estudiante estudiante_old = estudiantes[id];
        estudiantes[id] = estudiante;

        return Ok(estudiante_old);
    }

/*Lo ideal sería que esté en una clase de utilizades o algo similar*/
    private static Estudiante getRandomEstudiante(){

        Estudiante estudiante = new Estudiante();
        estudiante.nombre = nombres[Random.Shared.Next(0, nombres.Length)];
        estudiante.apellido = apellidos[Random.Shared.Next(0, apellidos.Length)];
        estudiante.id = Random.Shared.Next(1, 2000);
        return estudiante;
    }

    private static Dictionary<int, Estudiante> initializeEstudiantes(){
        Dictionary<int, Estudiante> estudiantes_dict = new Dictionary<int, Estudiante>();
            for (int i = 0; i < 5; i++) {
                Estudiante est = EstudiantesController.getRandomEstudiante();
                estudiantes_dict.Add(est.id, est);
            }
        return estudiantes_dict;
    }
}