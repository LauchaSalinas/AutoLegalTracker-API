using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Common
{
    public class DatabaseStartup
    {
        readonly ALTContext _context;
        readonly ActionBusiness _actionBusiness;
        public DatabaseStartup(ALTContext context, ActionBusiness actionBusiness)
        {
            _context = context;
            _actionBusiness = actionBusiness;
        }

        public async void InitializeWithData()
        {
            //// create UserTypes
            //var userTypeAdmin = new UserType
            //{
            //    Name = "SysAdmin",
            //    Description = "SysAdmin can do any action including the automated ones (System)",
            //    CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            //};

            //var userTypeSystem = new UserType
            //{
            //    Name = "System",
            //    Description = "System is the actor that represent all the automated actions that the software can do",
            //    CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            //};

            //var userTypeClientAdmin = new UserType
            //{
            //    Name = "Administrador",
            //    Description = "Administrador represents the client highest permission, should be only used by the Doctor/Attorney",
            //    CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            //};

            //var userTypeClientEmployee = new UserType
            //{
            //    Name = "Employee",
            //    Description = "This is simple example of a random UserType that the client creates for one of their employees to use the system in his behalf",
            //    CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            //};
            //_context.UserTypes.Add(userTypeClientAdmin);
            //_context.UserTypes.Add(userTypeClientEmployee);
            //_context.UserTypes.Add(userTypeAdmin);
            //_context.UserTypes.Add(userTypeSystem); // TODO switch to Enum
            //_context.SaveChanges();

            //// TODO SOtero/JGonzalez - since users have their user type we need to separate the user from the sign in credentials and Google Credentials.
            //// create a user
            //var user = new User
            //{
            //    Sub = "user123", // Example value for Sub
            //    Name = "Juan",
            //    FamilyName = "Gonzalez",
            //    Email = "juan@example.com",
            //    GoogleProfilePicture = "https://example.com/juan_profile.jpg",
            //    WebCredentialUser = "web_user", // Example value for WebCredentialUser
            //    WebCredentialPassword = "web_password", // Example value for WebCredentialPassword
            //    GoogleOAuth2RefreshToken = "refresh_token", // Example value for GoogleOAuth2RefreshToken
            //    GoogleOAuth2AccessToken = "access_token", // Example value for GoogleOAuth2AccessToken
            //    GoogleOAuth2TokenExpiration = 1234567890, // Example value for GoogleOAuth2TokenExpiration
            //    GoogleOAuth2TokenCreatedAt = DateTime.UtcNow, // Current UTC time as an example
            //    GoogleOAuth2IdToken = "id_token", // Example value for GoogleOAuth2IdToken
            //    userType = userTypeClientAdmin, // Assign the UserType navigation property
            //};

            //_context.Users.Add(user);
            //_context.SaveChanges();

            // create a LegalCase
            // var legalCase = new LegalCase
            // {
            //     Caption = "Caso 1",
            //     Description = "Caso 1",
            //     CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            //     User = user, // Assign the User navigation property
            //     Jurisdiction = "Example Jurisdiction", // Example value for Jurisdiction
            //     CaseNumber = "123", // Example value for CaseNumber
            // };
            // user.LegalCases.Add(legalCase);
            // _context.LegalCases.Add(legalCase);
            // _context.SaveChanges();

            // // create a LegalNotification
            // var legalNotification = new LegalNotification
            // {
            //     Title = "Nuevo Caso",
            //     Body = "Se desgina al medico a un nuevo caso",
            //     LegalCase = legalCase,
            //     To = "Medico",
            //     From = "Juez",
            //     CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            //     ActionHasBeenTaken = false
            // };
            // legalCase.LegalNotifications.Add(legalNotification);
            // _context.LegalNotifications.Add(legalNotification);
            // _context.SaveChanges();

            // // create LegalCaseAction
            // var legalCaseAction = new LegalCaseAction
            // {
            //     Name = "Action 1",
            //     Description = "Action 1",
            //     CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            //     User = user,
            //     UserTypes = { userTypeSystem }
            // };

            // var notificationConditionForNewCase = new NotificationCondition
            // {
            //     TitleContains = "Nuevo Caso",
            //     BodyContains = "Se desgina al medico a un nuevo caso"
            // };

            // legalCaseAction.NotificationCondition = notificationConditionForNewCase;

            // // create LegalCaseAttribute
            // var legalCaseAttribute = new LegalCaseAttribute
            // {
            //     Name = "New Case",
            //     Description = "Attribute for New Case",
            //     CreatedAt = DateTime.UtcNow // Example value for CreatedAt
            // };

            // legalCaseAction.LegalCaseAttributesToAdd.Add(legalCaseAttribute);

            // _context.LegalCaseActions.Add(legalCaseAction);


            var userWasSelectedCaseCondition = new LegalCaseCondition()
            {
                Name = "New Case",
                Description = "Attribute for New Case",
                CreatedAt = DateTime.UtcNow // Example value for CreatedAt
            };

            var userWasSelectedNotificationCondition = new NotificationCondition
            {
                TitleContains = "PERITO - Rta. del ULPIANO",
                BodyContains = "COZZI EMILIANO JAVIER"
            };



            var userWasSelectedAtt = new LegalCaseAttribute
            {
                Name = "Usuario fue seleccionado en sorteo",
                Description = "Resultado de desinsaculado involucra al usuario",
                CreatedAt = DateTime.UtcNow // Example value for CreatedAt
            };

            var userReceivedPositionAtt = new LegalCaseAttribute
            {
                Name = "Designacion de peritaje al usuario",
                Description = "Resultado de desinsaculado involucra al usuario",
                CreatedAt = DateTime.UtcNow // Example value for CreatedAt
            };

            var systemAcceptedPositionAtt = new LegalCaseAttribute
            {
                Name = "Cargo aceptado, usuario debe firmar",
                Description = "Borrador de respuesta cargado y turno asignado, esperando confirmacion del usuario",
                CreatedAt = DateTime.UtcNow // Example value for CreatedAt
            };

            var userWasSelectedAction = new LegalCaseAction
            {
                Name = "userWasSelectedAction",
                Description = "Action for New Case, User was assigned to the case",
                CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
                UserTypes = { _context.UserTypes.FirstOrDefault(x => x.Name == "System") },
                LegalCaseCondition = userWasSelectedCaseCondition,
                NotificationCondition = userWasSelectedNotificationCondition,
                LegalCaseAttributesToAdd = { userWasSelectedAtt }
            };

            //var userReceivedPositionAction = new LegalCaseAction
            //{
            //    Name = "userReceivedPositionAction",
            //    Description = "Action for Case, User reply is required",
            //    CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            //    UserTypes = { _context.UserTypes.FirstOrDefault(x => x.Name == "System") },
            //    LegalCaseCondition = newCaseCondition,
            //    NotificationCondition = newCaseNotificationCondition,
            //    LegalCaseAttributesToAdd = { userReceivedPositionAtt },
            //    LegalCaseAttributesToDelete = { userWasSelectedAtt }
            //};

            _context.SaveChanges();

        }
    }
}
