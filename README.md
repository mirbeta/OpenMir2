Legend of Mir 2 complete server, support classic 1.76 (reloaded) function, support online and multiplayer entertainment.

This project refers to the network Delphi code, which can be used with the original 1.76 source code of Legend of Blood for game experience. If you have any questions or problems, please submit Issues.  

### How to use    
## Client
    *  You can build a game client using the Dephi source code you can also choose the game client we build for you.  
* How to download the Delphi game client  
    * Please use the search engine to search for blood legend source code yourself.  
* Use our game client  
    * You can download here (later, we are still preparing, if you can't wait for the experience, you can consider self-compilation of the game client, of course, this requires you to experience Delphi experience)  

## Server
    *  You can download a variety of game versions on your web (of course there will be a variety of game issues, such as: Script commands cannot be supported, etc.) or download the foundation version of us.  
* You can download here (later, we are ready, you can also use the search engine search yourself, once we are ready, we will provide relevant documents immediately)

### Startup sequence and project description. 
1. DBSvr (database service, responsible for data storage). 
2. LoginSvr (account login service, responsible for account registration, login, server selection, etc.). 
3. GameSvr (game data engine, responsible for game data processing interaction, spell casting, walking, etc.). 
4. GameGate (game gateway, responsible for sending player data to the game engine, the data interacted by players are all in this service and then forwarded to the data engine for processing). 
5. SelGate (role gateway, responsible for role query, new creation, deletion, etc., and finally processed by DBSvr, the service will not be able to obtain character data after the client logs in if the service is not activated). 
6. LoginGate (login gateway, responsible for forwarding the login data to LoginSvr for processing, the service does not start the client and cannot connect to the game). 

### images
![](./Images/1632561445962.jpg)
![](./Images/1632561467819.jpg)
![](./Images/1632561488323.jpg)
![](./Images/1632561522104.jpg)
