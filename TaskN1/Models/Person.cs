using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;
using TaskN1.Migrations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace TaskN1.Models
{
    public class Person
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "გთხოვთ მიუთითოთ სახელი")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "სიმბოლოების რაოდენობა, მინ-2, მაქს-50")]
        [RegularExpression("(^[a-zA-Z]+$)|(^[ა-ჰ]+$)", ErrorMessage = "გთხოვთ შეიყვანოთ მხოლოდ ქართული ან მხოლოდ ინგლისური სიმბოლოები")]
        [Display(Name = "სახელი")]
        public string Name { get; set; }
        //ტექსტური, სავალდებულო, მინიმუმ 2 და მაქსიმუმ 50 სიმბოლო, უნდა შეიცავდეს მხოლოდ ქართული    




        [Required(ErrorMessage = "გთხოვთ მიუთითოთ გვარი")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "სიმბოლოების რაოდენობა, მინ-2, მაქს-50")]
        [RegularExpression("(^[a-zA-Z]+$)|(^[ა-ჰ]+$)", ErrorMessage = "გთხოვთ შეიყვანოთ მხოლოდ ქართული ან მხოლოდ ინგლისური სიმბოლოები")]
        [Display(Name = "გვარი")]
        public string Surname { get; set; }
        //ან ლათინური ანბანის ასოებს, არ უნდა შეიცავდეს ერთდროულად ლათინურ და ქართულ ასოებს



        [Required(ErrorMessage = "გთხოვთ მიუთითოთ სქესი")]
        [Display(Name = "სქესი")]
        public string Sex { get; set; }           // Male / Female




        [Required(ErrorMessage = "გთხოვთ მიუთითოთ თქვენი პირადობის ნომერი")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "პირადობის ნომერი შედგება 11 ციფრისგან")]
        [Display(Name = "პირადობის ნომერი")]
        [RegularExpression("^[0-9]{0,11}$", ErrorMessage = "გთხოვთ შეიყვანოთ ციფრები")]
        public string PersonalID { get; set; } //ტექსტური, სავალდებულო, 11 ციფრი

      

        [Display(Name = "დაბადების თარიღი")]
        [Required(ErrorMessage = "გთხოვთ მიუთითოთ დაბადების თარიღი")]
        [DataType(DataType.Date)]
        public DateTime PersonBirthDate { get; set; }

        [Display(Name = "დაბადების ქალაქი")]
        public string City { get; set; }  // ქალაქის იდენტიფიკატორი, ქალაქების ცნობარიდან

        [Display(Name = "მობილურის ნომერი")]
        [Phone(ErrorMessage = "მითითებული ნომერი არასწორია")]
        public string Mobile { get; set; }   // ნომრის ტიპი (დასაშვები მნიშვნელობები: მობილური, ოფისის, სახლის)



        [Display(Name = "სურათი")]
        public string Picture { get; set; }  //ტექსტური, ფაილის relative მისამართი ფაილურ სისტემაში
        [Display(Name = "კავშირი")]
        public string ConnectedPeople { get; set; }  //კავშირის ტიპი (დასაშვები მნიშვნელობები: კოლეგა, ნაცნობი, ნათესავი, სხვა)
                                                     //დაკავშირებული ფიზიკური პირის იდენტიფიკატორი
        
       // public string ImageName { get; set; }

        [NotMapped]
        [Required(ErrorMessage ="გთხოვთ ატვირთოთ სურათი")]
        public IFormFile ImageFile { get; set; }
    }


}
