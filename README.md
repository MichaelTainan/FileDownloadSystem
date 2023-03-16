# Socket TCP Download System

## 1. Test-Driven Development
This project creating followed the TDD concept to build. Use Nunit+Mock(Moq) to create the testing methods, then return to create in the main projects. Almost follow it, but sometimes when I want to create quickly, return to traditional way to build, then add the testing methods.

## 1.1 ServerTCPNUnitTest
### 1.1.1 Models 
This folders include the three manager class unit test
### 1.1.2 ViewModels
This folders include two viewmodel class unit test, espically in ServerViewModelTest.cs, use Mock(Moq) to fake the ListenManager to response to assert comparison.

## 1.2 ClientTCPNUnitTest
This layer include two manager class unit test

## 2. ServerTCP
Use the MVVM Design Pattern to Create  the server site desktop program, listen the client request, then response the results(message or download file). The architecture follows as below

### 2.1 View layer
#### 2.1.1 MainWindow.xaml
Use WPF to create, include ListView to list the client IP, Port, request file name and Create/Stop Server service button. They all binding with the VM program to auto refresh the contents.

### 2.2 ViewModel layer
#### 2.2.1 ClientViewModel.cs
Use to control the clientInfo's properties(data) when they changed. and that ServerVieModel program use this class type to collection clinets.
#### 2.2.2 ServerViewModel.cs
Like the postman to deliver the objects from ListenManager.cs in Models layer to mainWindow.xaml.cs in View layer. When ListenManager's event handlers send the action, this vm program will call the view progam had change it's binging objects' contents, like change file name or add/delete the connection infomation in listView, and control start/stop buttons. 

### 2.3 Models layer
#### 2.3.1 FileManager.cs
Use to responsible for the file upload job, include search the file if is in the server site's folder. If find the file then return the file, or retun null when can't find the file.
#### 2.3.2 DisplayManager.cs
Use to responsible for the clientInfo object display, It's is created when I used TDD to disign, but finally had not used. But I still keey it maybe used when refactoring. 
#### 2.3.3 ListenManager.cs
Use to responsible for the Server site TCP service. It is the main service in the project. It control the client connection, and response the result to client. 

## 3. ClientTCP
Use the simple architecture to design the project. As follows.

### 3.1 MainWindow.xaml and MainWindow.xaml.cs
It's client destop main program. Responsible for user sit interactive. Send the text box contents to ConnectManager program and display information about clinet and server site message. 

### 3.2 RecordManager.cs
Reponsible for recording the serverInfo from MainWindow.xaml.cs or ConnectManager.cs, let the two programs can refer the correct data.

### 3.3 ConnectManager.cs
Use to responsible for control the client TCP behavior. Connects to the client site and send/receive message, then update serverinfo object, let MainWindow.xaml.cs can display the response message.

## 4. Version
v1.0: initialize nad release the project programs