using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Business
{
    public class ConditionBusiness
    {
        public ConditionBusiness() { }
        public bool CheckLegalCaseCondition (LegalCaseCondition condition, LegalCase legalCase)
        {
            return true;
            // TODO implement this method
        }

        public bool CheckLegalNotificationCondition(NotificationCondition condition, LegalNotification legalNotification)
        {
            if (condition.TitleContains != null)
            {
                if (!legalNotification.Title.Contains(condition.TitleContains))
                {
                    return false;
                }
            }

            if (condition.BodyContains != null)
            {
                if (!legalNotification.Description.Contains(condition.BodyContains))
                {
                    return false;
                }
            }

            if (condition.TitleDoesNotContain != null)
            {
                if (legalNotification.Title.Contains(condition.TitleDoesNotContain))
                {
                    return false;
                }
            }

            if (condition.BodyDoesNotContain != null)
            {
                if (legalNotification.Description.Contains(condition.BodyDoesNotContain))
                {
                    return false;
                }
            }

            if (condition.ToContains != null)
            {
                if (!legalNotification.To.Contains(condition.ToContains))
                {
                    return false;
                }
            }

            if (condition.ToDoesNotContain != null)
            {
                if (legalNotification.To.Contains(condition.ToDoesNotContain))
                {
                    return false;
                }
            }

            if (condition.FromContains != null)
            {
                if (!legalNotification.From.Contains(condition.FromContains))
                {
                    return false;
                }
            }

            if (condition.FromDoesNotContain != null)
            {
                if (legalNotification.From.Contains(condition.FromDoesNotContain))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
