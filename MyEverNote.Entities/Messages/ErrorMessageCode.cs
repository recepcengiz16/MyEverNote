
namespace MyEverNote.Entities.Messages
{
    //Enumda kütüphaneler olmaz. Hata mesajlarını kodlayarak oluşturmaya çalıştık. 
    public enum ErrorMessageCode
    {
        UserNameAlreadyExists=101,
        EmailAlreadyExists=102,
        UserIsNotActive=151,
        UserNameOrPassWrong=152,
        CheckYourEmail=153,
        UserAlreadyActive=154,
        ActivateIdDoseNotExist=155,
        UserNotFound=156,
        ProfilCouldNotUpdate = 157,
        UserCouldNotRemove = 158,
        UserCouldNotFind = 159,
        UserCouldNotInserted = 160,
        UserCouldNotUpdated = 161
    }
}
