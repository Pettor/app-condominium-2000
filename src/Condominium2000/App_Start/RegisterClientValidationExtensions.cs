using Condominium2000;
using DataAnnotationsExtensions.ClientValidation;
using WebActivator;

[assembly: PostApplicationStartMethod(typeof(RegisterClientValidationExtensions), "Start")]
 
namespace Condominium2000 {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}