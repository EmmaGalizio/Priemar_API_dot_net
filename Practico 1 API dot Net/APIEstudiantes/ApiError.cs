
public class ApiError{

    public Dictionary<string, string> errors { get;set; }

    public int statusCode;


    public void addError(string error, string key) {
        
        if (errors == null) errors = new Dictionary<string, string>();
        errors.Add(key, error);
    
    }

}