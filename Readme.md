# CLI Communication with RabbitMQ and RabbitMQ.Client.

This project is an example of simple communication between 3 CLI Projects.
 
Solution contains 4 projects -
 
- RabbitMqShared - contains shared configuration, models, etc
- MessagePublisher - publish messages that is written by user in command line
- SimpleMessageConsumer - outputs received messages
- ModificationMessageConsumer - outputs received messages, and sends modification requests if received message starts or ends with whitespace. If a message starts with whitespace it should be modified for everyone. If a message ends with whitespace and doesn't start with whitespace, it should be modified only for the ModificationMessageConsumer project.
 
Configuration:
- RabbitMqShared/rabbitmqSharedConfig.json - contains credentials, host, port and virtual scheme of RabbitMQ server. Additionally contains name of message publish exchange and name of queue that will be used for modification.
- SimpleMessageConsumer/appsetting.json and ModificationMessageConsumer/appsetting.json contain names of queues for received messages.
 
Communication diagrams -
 
- Message publishing
![Message publish](images/PublishingMessage.JPG)
 
- Modify request
![Modify request](images/ModifyRequest.JPG)
 
- Modify response for everyone
![Modify response for everyone](images/PublishingMessage.JPG)
 
- Modify response only for ModificationMessageConsumer
![Modify response only for ModificationMessageConsumer](images/SingleModifyMessage.JPG)