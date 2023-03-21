# Socket TCP Download System
## 1. Requirement
### 1.1. A client desktop application send requests to a server desktop application. The request is a file downloading request.
### 1.2. Connection is based on TCP (Socket) without any 3-party library.
### 1.3. Client desktop functions:
#### a. A button to start sending request
#### b. A textbox records the server ip
#### c. A textbox records the server port
#### d. A textbox records the file name
#### e. A textbox records file path to save in the client computer
#### f. A textbox shows the successful or error messages when sending requests.
### 1.4. Server desktop functions:
#### a. A textbox displays current client ip of the request
#### b. A textbox displays current client port of the request
#### c. A textbox displays current file name of request
#### d. If request’s file found, send the file to client. Or send error message to client.

## 2. Test-Driven Development
This project creating followed the TDD concept to build. Use Nunit+Mock(Moq) to create the testing methods, then return to create in the main projects. Almost follow it, but sometimes when I want to create quickly, return to traditional way to build, then add the testing methods.

## 2.1. ServerTCPNUnitTest
### 2.1.1. Models 
This folders include the three manager class unit test
### 2.1.2. ViewModels
This folders include two viewmodel class unit test, espically in ServerViewModelTest.cs, use Mock(Moq) to fake the ListenManager to response to assert comparison.

## 2.2. ClientTCPNUnitTest
This layer include two manager class unit test

## 3. ServerTCP
Use the MVVM Architecture Pattern to Create  the server site desktop program, listen the client request, then response the results(message or download file). The architecture follows as below

### 3.1. View layer
#### 3.1.1. MainWindow.xaml
Use WPF to create, include ListView to list the client IP, Port, request file name and Create/Stop Server service button. They all binding with the VM program to auto refresh the contents.

### 3.2. ViewModel layer
#### 3.2.1. ClientViewModel.cs
Use to control the clientInfo's properties(data) when they changed. and that ServerVieModel program use this class type to collection clinet.
#### 3.2.2. ServerViewModel.cs
Like the postman to deliver the objects from ClientManager and ListenManager in Models layer to mainWindow.xaml.cs in View layer. When ClientManager's event handlers send the action, this viewmodel program will call the view progam had change it's binging objects' contents, like change file name or add/delete the clients' elements  in listView, and control start/stop buttons. 

### 3.3. Models layer
#### 3.3.1. FileManager.cs
Use to responsible for the file upload job, include search the file if is in the server site's folder. If find the file then return the file, or retun null when can't find the file.
#### 3.3.2. ClientManager.cs
Use to responsible for the clientInfo object management, about add/update/remove behavior and send message to clinet. this class is refactoring form ListenManager to follow SRP. 
#### 3.3.3. ListenManager.cs
Use to responsible for the Server site TCP service. It is the main service in the project. Inject IFileManager and IClientManager to follow DIP. Now It just focuses on client connection/disconnection and Send the request file, other tasks moved to other manager classes. 

## 4. ClientTCP
Use the simple architecture to design the project. As follows.

### 4.1. MainWindow.xaml and MainWindow.xaml.cs
It's client destop main program. Responsible for user sit interactive. Send the text box contents to ConnectManager program and display information about clinet and server site message. 

### 4.2. RecordManager.cs
Reponsible for recording the serverInfo from MainWindow.xaml.cs or ConnectManager.cs, let the two programs can refer the correct data.

### 4.3. ConnectManager.cs
Use to responsible for control the client TCP behavior. Connects to the client site and send/receive message, then update serverinfo object, let MainWindow.xaml.cs can display the response message.

## 5. Version
### v1.0 : Initialize nad release the project programs
### v1.5 : Refactoring the ServerTCP to follow SRP and DIP principle.      
### v1.51: Fixed the ClientTCP exception process and return the message to display in the message box.