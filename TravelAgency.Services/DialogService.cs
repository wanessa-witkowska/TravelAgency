using TravelAgency.Controls;
using TravelAgency.Interfaces;

namespace TravelAgency.Services;

public class DialogService : IDialogService
{
    public bool? Show(string itemName)
    {
        ConfirmationDialog confirmationDialog = new ConfirmationDialog(itemName);
        return confirmationDialog.ShowDialog();
    }
}
