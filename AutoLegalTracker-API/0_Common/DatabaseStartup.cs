using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Common
{
    public class DatabaseStartup
    {
        public readonly ALTContext _context;
        public DatabaseStartup(ALTContext context)
        {
            _context = context;
        }

        public void InitializeWithData()
        {
            // create UserTypes
            var userTypeAdmin = new UserType
            {
                Name = "SysAdmin",
                Description = "SysAdmin can do any action including the automated ones (System)",
                CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            };

            var userTypeSystem = new UserType
            {
                Name = "System",
                Description = "System is the actor that represent all the automated actions that the software can do",
                CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            };

            var userTypeClientAdmin = new UserType
            {
                Name = "Administrador",
                Description = "Administrador represents the client highest permission, should be only used by the Doctor/Attorney",
                CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            };

            var userTypeClientEmployee = new UserType
            {
                Name = "Employee",
                Description = "This is simple example of a random UserType that the client creates for one of their employees to use the system in his behalf",
                CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
            };

            // TODO SOtero/JGonzalez - since users have their user type we need to separate the user from the sign in credentials and Google Credentials.
            // create a user
            var user = new User
            {
                Sub = "user123", // Example value for Sub
                Name = "Juan",
                FamilyName = "Gonzalez",
                Email = "juan@example.com",
                GoogleProfilePicture = "https://example.com/juan_profile.jpg",
                WebCredentialUser = "web_user", // Example value for WebCredentialUser
                WebCredentialPassword = "web_password", // Example value for WebCredentialPassword
                GoogleOAuth2RefreshToken = "refresh_token", // Example value for GoogleOAuth2RefreshToken
                GoogleOAuth2AccessToken = "access_token", // Example value for GoogleOAuth2AccessToken
                GoogleOAuth2TokenExpiration = 1234567890, // Example value for GoogleOAuth2TokenExpiration
                GoogleOAuth2TokenCreatedAt = DateTime.UtcNow, // Current UTC time as an example
                GoogleOAuth2IdToken = "id_token", // Example value for GoogleOAuth2IdToken
                userType = userTypeClientAdmin, // Assign the UserType navigation property
            };

            // create a LegalCase
            var legalCase = new LegalCase
            {
                Caption = "Caso 1",
                Description = "Caso 1",
                CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
                User = user, // Assign the User navigation property
                Jurisdiction = "Example Jurisdiction", // Example value for Jurisdiction
                CaseNumber = "123ABC", // Example value for CaseNumber
            };
            user.LegalCases.Add(legalCase);

            // create a LegalNotification
            var legalNotification = new LegalNotification
            {
                Title = "Nuevo Caso",
                Description = "Se desgina al medico a un nuevo caso",
                LegalCase = legalCase,
                To = "Medico",
                From = "Juez"
            };

            var notificationConditionForNewCase = new NotificationCondition
            {
                TitleContains = "Nuevo Caso",
                BodyContains = "Se desgina al medico a un nuevo caso"
            };



            // create LegalCaseAction
            var legalCaseAction = new LegalCaseAction
            {
                Name = "Action 1",
                Description = "Action 1",
                CreatedAt = DateTime.UtcNow // Example value for CreatedAt
            };

            // create LegalCaseActionUserType
            var legalCaseActionUserType = new LegalCaseAction_UserType
            {
                UserType = userTypeClientAdmin,
                LegalCaseAction = legalCaseAction
            };

            legalCaseAction.UserTypeAllowedToUseAction.Add(legalCaseActionUserType);

            legalCaseAction.NotificationCondition = notificationConditionForNewCase;
            legalCase.LegalNotifications.Add(legalNotification);
            // create LegalCaseAttribute
            var legalCaseAttribute = new LegalCaseAttribute
            {
                Name = "New Case",
                Description = "Attribute for New Case",
                CreatedAt = DateTime.UtcNow // Example value for CreatedAt
            };

            // create LegalCaseActionAttribute
            var legalCaseActionAttribute = new LegalCaseAction_LegalCaseAttribute
            {
                LegalCaseAction = legalCaseAction,
                LegalCaseAttribute = legalCaseAttribute,
                Add = true
            };

            legalCaseAction.LegalCaseAttributesToAddOrDelete.Add(legalCaseActionAttribute);

            // Simulate scraping the web
            var conditions = new List<NotificationCondition>();
            conditions.Add(notificationConditionForNewCase);
            var conditionsAreNotMet = false;
            foreach (var condition in conditions)
            {
                if (condition.TitleContains != null)
                {
                    if (!legalNotification.Title.Contains(condition.TitleContains))
                    {
                        conditionsAreNotMet = true;
                    }
                }

                if (condition.BodyContains != null)
                {
                    if (!legalNotification.Description.Contains(condition.BodyContains))
                    {
                        conditionsAreNotMet = true;
                    }
                }

                if (condition.TitleDoesNotContain != null)
                {
                    if (legalNotification.Title.Contains(condition.TitleDoesNotContain))
                    {
                        conditionsAreNotMet = true;
                    }
                }

                if (condition.BodyDoesNotContain != null)
                {
                    if (legalNotification.Description.Contains(condition.BodyDoesNotContain))
                    {
                        conditionsAreNotMet = true;
                    }
                }

                if (condition.ToContains != null)
                {
                    if (!legalNotification.To.Contains(condition.ToContains))
                    {
                        conditionsAreNotMet = true;
                    }
                }

                if (condition.ToDoesNotContain != null)
                {
                    if (legalNotification.To.Contains(condition.ToDoesNotContain))
                    {
                        conditionsAreNotMet = true;
                    }
                }

                if (condition.FromContains != null)
                {
                    if (!legalNotification.From.Contains(condition.FromContains))
                    {
                        conditionsAreNotMet = true;
                    }
                }

                if (condition.FromDoesNotContain != null)
                {
                    if (legalNotification.From.Contains(condition.FromDoesNotContain))
                    {
                        conditionsAreNotMet = true;
                    }
                }

            }

            if (!conditionsAreNotMet)
            {
                if (legalCaseAction.LegalResponseTemplateId != null)
                {
                    // send a response
                }

                if (legalCaseAction.AssignMedicalAppointment)
                {
                    // assign a medical appointment
                }

                if (legalCaseAction.EmailTemplateId != null)
                {
                    // send an email
                }

                if (legalCaseAction.LegalCaseAttributesToAddOrDelete.Count > 0)
                {
                    foreach (var attribute in legalCaseAction.LegalCaseAttributesToAddOrDelete)
                    {
                        if (attribute.Add)
                        {
                            legalNotification.LegalCase.LegalCaseAttributes.Add(attribute.LegalCaseAttribute);
                        }
                        else
                        {
                            legalNotification.LegalCase.LegalCaseAttributes.Remove(attribute.LegalCaseAttribute);
                        }
                    }
                }
            }

            // store it in the database
            _context.UserTypes.Add(userTypeClientAdmin);
            _context.UserTypes.Add(userTypeClientEmployee);
            _context.UserTypes.Add(userTypeAdmin);
            _context.UserTypes.Add(userTypeSystem); // TODO switch to Enum
            _context.Users.Add(user);
            _context.LegalCases.Add(legalCase);
            _context.LegalNotifications.Add(legalNotification);
            _context.LegalCaseActions.Add(legalCaseAction);
            _context.LegalCaseAttributes.Add(legalCaseAttribute);
            _context.NotificationConditions.Add(notificationConditionForNewCase);


            _context.SaveChanges();
        }
    }
}
