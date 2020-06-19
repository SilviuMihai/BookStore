
function confirmDelete(uniqueId, isDeleteClicked)
{
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmationDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        //using jquery selector $
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    }
    else
    {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }   
}