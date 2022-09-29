Create Procedure DeleteRentedBoard
@UserID NVarChar(MAX)
as
Begin
  Delete from Rent where UserID = @UserID
End