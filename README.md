# support-chat
Application demonstrate the Rabbbit MQ usage.

User the below command to download the docker image for RabbitMQ
docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management

On successful completion visit url http://localhost:15672/. This is the RabbitMQ management portal.
Default username and password both is set to 'guest'

Below is the project structure :
<br>

![image](https://user-images.githubusercontent.com/4363523/191039409-9d94f75a-4265-4305-b78d-e20287d05743.png)

<ul>
<li>Client.App.CLI - demonstrate the chat client</li>
<li>Agent.App.CLI - demonstrate the support chat agent</li>
</ul>

