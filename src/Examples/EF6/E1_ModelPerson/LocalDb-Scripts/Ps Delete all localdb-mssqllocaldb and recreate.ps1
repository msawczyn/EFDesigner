Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted -Force;

write-host "--WORK IN PROGRESS - Attempt to Drop and Recreate Database"


$DatabaseName ="EFVisualExamples"
$localUser = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::UserProfile)

write-host "Stopping Server Instance, so connections drop..."
sqllocaldb stop mssqllocaldb

$oReturn=[System.Windows.Forms.MessageBox]::Show("Delete Server Instance: mssqllocaldb","Delete mssqllocaldb?",[System.Windows.Forms.MessageBoxButtons]::OKCancel) 
switch ($oReturn){
    "OK" {
        write-host "Deleting Server Instance..."
        sqllocaldb d mssqllocaldb
    } 
    "Cancel" {
        write-host "Server Instance retained"
    } 
}




$mdfFile = ($localUser + "\\" + ($DatabaseName.mdf)  )
$mdfName = ($DatabaseName + "_dat")
Remove-Item -path $mdfFile

$extnldf = "_log.ldf"
$ldfName = ($DatabaseName + "_log")
$ldfFile = ($localUser+ "\" + ($DatabaseName + $extnldf))
Remove-Item -path ldfFile


$myserver = "(localdb)\MSSQLLocalDB"

write-host "Starting Server Instance..."

Sqlcmd -S $myserver -Q "Select @@servername" 

write-host "Creating new Database on Server Instance..."

Sqlcmd -S $myserver -Q "USE master;  
                        GO  
                        CREATE DATABASE $DatabaseName
                        ON   
                        ( NAME = $mdfName,  
                            FILENAME = '$mdfFile',  
                            SIZE = 10,  
                            FILEGROWTH = 5 )  
                        LOG ON  
                        ( NAME = $ldfName,  
                            FILENAME = '$ldfFile',  
                            SIZE = 5MB,  
                            FILEGROWTH = 5MB );  
                        GO  "