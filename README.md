![Logo](https://raw.githubusercontent.com/ramon54321/SharpNetwork/master/Documentation/icon/Wide@0.5x-100.jpg)

# SharpNetwork
A simple TCP networking system for .Net.

## Getting Started
View the [example](https://github.com/ramon54321/SharpNetwork/tree/master/SharpNetworkExample) project for more information.

## Overview
The project is intended to present a very simple tcp client / server setup. This can be used as boilerplate to create more complex systems, or be used as is.

Created with Unity in mind, Net35 targeted dlls are available in releases. NetStandard dlls are also provided for other projects.

## Contributing
If you'd like to contribute, feel free to fork the repository. Pull requests are welcome!

## Diagrams
#### Class Diagram
![Class Diagram](http://repo.ramonbrand.ml/images/SharpNetwork/ClassDiagram.svg)

## Dev Log
### 12 Jan 2018
Core server and client now works as intended.
 - Server starts and listens for connections
 - Client starts and tries to connect to given host and port
 - Server accepts client and adds creates new ServerClient object to contain client data
 - Server adds the new ServerClient to its clients list
 
 The server has a list of all clients, can send each client a byte[] and can receive byte[] from each client.
 Client can send and receive byte[] to / from server.
 
 The next step will be to architect and implement a system to deal with sending and receiving data in a more useful manner than just bytes.

### 11 Jan 2018
Started the project by creating 3 simple classes, namely, NetworkServer, which acts as the server, NetworkClient, which acts as the client controller, and finally ServerClient, which stores information and the TcpClient of each client on the server.

Need to work on some sort of abstraction to allow easy overrides of event methods such as OnConnect and OnMessageReceived.
