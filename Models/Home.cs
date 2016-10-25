using System.ComponentModel.DataAnnotations;

namespace theWall.Models
{

 public abstract class BaseEntity {}
 public class User : BaseEntity
 {
  public int id { get; set; }
  [Required]
  [MinLength(2)]
  public string first_name { get; set; }
  [Required]
  [MinLength(2)]  
  public string last_name { get; set; }
  [Required]
  [EmailAddress]
  public string email { get; set; }
  [Required]
  [MinLength(8)]
  public string password { get; set; }
  [Required]
  [MinLength(8)]
  [Compare("password", ErrorMessage="Passwords must match.")]
  public string confirm_password { get; set; }

  public string created_at { get; set; }
  public string updated_at { get; set; }
  
 }

 public class Message : BaseEntity
 {
     public int id { get; set; }
     [Required]
     [MinLength(5)]
     public string message { get; set; }
     public string first_name { get; set; }
     public string created_at { get; set; }
     public string updated_at { get; set; }
     

 }

 public class Comment : BaseEntity
 {
     public int message_id { get; set; }
     public string comment { get; set; }
     public string user_id { get; set; }
     public string first_name { get; set; }
     public string created_at { get; set; }
 }

}