using Newtonsoft.Json;  
  
namespace EventApp.Models  
{  
   
    public partial class UserProfile  
    {  
  
        [JsonProperty("email")]  
        public string Email { get; set; }  
   
  
        [JsonProperty("name")]  
        public string Name { get; set; }   

    }  
}  
