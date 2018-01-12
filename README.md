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
### 11 Jan 2018
Started the project by creating 3 simple classes, namely, NetworkServer, which acts as the server, NetworkClient, which acts as the client controller, and finally ServerClient, which stores information and the TcpClient of each client on the server.

Need to work on some sort of abstraction to allow easy overrides of event methods such as OnConnect and OnMessageReceived.
