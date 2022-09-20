# support-chat
Application demonstrate the Rabbbit MQ usage.

# Solution Setup

<b>RabbitMQ Configuration</b>
</br>
User the below command to download the docker image for RabbitMQ 
<br>
docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management

On successful completion visit url http://localhost:15672/. This is the RabbitMQ management portal.
Default username and password both is set to 'guest'
<br>

![image](https://user-images.githubusercontent.com/4363523/191338791-dd746f68-e212-4dba-9e13-a0963462aaa1.png)

<b>DB Configuration</b>
</br>
In the Visual Studio Package Manager Console, select the AgentCoordication.CLI and run the below commands :

<ul>
<li>Install-Package Microsoft.EntityFrameworkCore.Tools</li>
<li>Add-Migration InitialCreate</li>
<li>Update-Database</li>
</ul>

Initial data is added with the seed method.<br>
To explore the SqlLite DB use https://sqlitebrowser.org/dl/.<br>

Below is the project structure :
<br>

![image](https://user-images.githubusercontent.com/4363523/191039409-9d94f75a-4265-4305-b78d-e20287d05743.png)

<ul>
<li>Client.App.CLI - demonstrate the chat client</li>
<li>Agent.App.CLI - demonstrate the support chat agent</li>
</ul>

