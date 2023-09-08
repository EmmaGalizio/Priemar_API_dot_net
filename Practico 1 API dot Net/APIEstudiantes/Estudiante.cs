
public class Estudiante{

    public int id  { get; set; }
    public string apellido  { get; set; }
    public string nombre  { get; set; }
    public DateOnly fechaNacimiento  { get; set; }
    public string email  { get; set; }


    public Dictionary<string, string> validate_new() {

        Dictionary<string, string> errors = new Dictionary<string, string>();

        if (apellido == null || apellido.Length == 0 || apellido.Length > 255) errors.Add("APELLIDO", "Es necesario ingresar un apellido no vac�o de no m�s de 255 caracteres");
        if (nombre == null || nombre.Length == 0 || nombre.Length > 255) errors.Add("NOMBRE", "Es necesario ingresar un nombre no vac�o de no m�s de 255 caracteres");
        if (email == null || email.Length == 0) errors.Add("EMAIL", "Campo obligatorio");
        if (email.Length > 255 || !validate_email(email)) errors.Add("EMAIL", "El email provisto no tiene el formato correcto");
        /*if (fechaNacimiento == null || fechaNacimiento.CompareTo(new DateOnly()) <= 0 || fechaNacimiento.CompareTo(new DateOnly(1990, 1, 1)) > 0) {
            errors.Add("FECHA_NACIMIENTO", "La fecha de nacimiento provista no es v�lida");
        }*/

        return errors.Count > 0  ? errors : null;

    }

    private Boolean validate_email(string email){


        string[] parts = email.Split("@");
        if (parts.Length != 2 || parts[0].Length > 200 || parts[1].Length > 55  || parts[0].Length == 0 || parts[1].Length == 0) {
            return false;
        }
        return true;

    }

}