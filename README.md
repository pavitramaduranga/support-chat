# Chat Initiator Application
Application demonstrate the Rabbbit MQ usage.

## Application Architecture 
<br>

![Final Diagram1 drawio](https://user-images.githubusercontent.com/4363523/191801009-b80a7fa5-1dae-46f7-8909-0bb4a95fc2ae.png)

## Tech Stack

<ul>
<li>.Net 6</li>
<li>RabbitMQ</li>
<li>SqlLite DB</li>
<li>Docker</li>
</ul>

## Solution Setup

### RabbitMQ Configuration
</br>
Use the below command to pull the docker image for RabbitMQ 
<br>
<i>docker pull rabbitmq:3-management</i>
</br>
Use the below command to run the docker image. This will map mort 15672 for the management web app and port 5672 for the message broker. 
<br>
<i>docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management</i>

On successful completion visit url http://localhost:15672/. This is the RabbitMQ management portal.
Default username and password both is set to 'guest'
<br>

![image](https://user-images.githubusercontent.com/4363523/191338791-dd746f68-e212-4dba-9e13-a0963462aaa1.png)

### DB Configuration
</br>
In the Visual Studio Package Manager Console, select the AgentCoordication.CLI and run the below commands :

<ul>
<li>Install-Package Microsoft.EntityFrameworkCore.Tools</li>
<li>Add-Migration InitialCreate</li>
<li>Update-Database</li>
</ul>

Initial data is added with the seed method.<br>
To explore the SqlLite DB use https://sqlitebrowser.org/dl/.<br>

Right click on the AgentCoordication.CLI. Go to properties and then click debug. Browse and set the working directory to the project folder.
This is to set the path of the SQLLite DB.
<br>
### Project Structure
<br>

![image](https://user-images.githubusercontent.com/4363523/191039409-9d94f75a-4265-4305-b78d-e20287d05743.png)
<br>
<ul>
<li>Client.App.CLI - demonstrate the chat client</li>
<li>Agent.App.CLI - demonstrate the support chat agent</li>
</ul>

## Future Work

<br>
<ul>
<li>Move the RabbitMQ logics to a diferent project and implement Apaptor patern</li>
<li>Remove the identified inactive users from polling</li>
<li>Improve the administrator endpoints to start a session</li>
</ul>
<br>

## Testing

Below is how the load will be balanced during an office hour.

<li>
  <ul>1 Junior -> 1 * 10 * 0.4 * 1.5 = 6 (queue capacity)</ul>
  <ul>2 Mid Level -> 2 * 10 * 0.6 * 1.5 = 18 (queue capacity)</ul>
  <ul>1 Team Lead -> 1 * 10 * 0.5 * 1.5 = 7 (queue capacity)</ul>
  <ul>6 Overflow -> 6 * 10 * 0.4 * 1.5 = 36 (queue capacity)</ul>
</li>
<br>
Below diagram shows how this us disttibuted among agent queues.
<br>
The requests after the maximum queue length will not be added to the Session queue.
<br>

![image](https://user-images.githubusercontent.com/4363523/191839670-e74d4330-a643-4277-885e-d05e1a64b275.png)


