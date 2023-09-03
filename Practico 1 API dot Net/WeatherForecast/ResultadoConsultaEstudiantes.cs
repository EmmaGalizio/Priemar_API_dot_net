
public class ResultadoConsultaEstudiantes{

    public int page  { get; set; }
    
    public int size  { get; set; }
    public string next  { get; set; }
    public IEnumerable<Estudiante> estudiantes  { get; set; }


}