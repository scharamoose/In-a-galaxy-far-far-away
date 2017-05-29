In A Galaxy Far Far Away
========================

Overview
--------
The solution satisfies the requirements of the coding challenge by utilising a number of asynchronous api calls.  

Once each response is received, the number of re-supplies required for each starship to travel the distance provided is then calculated.  There are also a number of unit tests included which assert various inputs, valid or otherwise.  

In addition, I have included a test to verify that the URL supplied returns a successful response. 

Running the console application
-------------------------------
You may run the application from either the IDE or you can locate the **InAGalaxyFarFarFarAway** application under the bin folder for your chosen solution configuration. 

<p align="center">
  <img src="./images/application.png">
</p>

Usage
-----
Once the application starts, the user will be prompted to enter the distance to travel in MGLT.  

<p align="center">
  <img src="./images/consolePrompt.png">
</p>

Assuming the input entered is valid, the application will proceed to request information via the Star Wars API a.k.a SWAPI.  

<p align="center">
  <img src="./images/console1million.png">
</p>

If input is invalid, the user will prompted to re-enter a valid input, along with a further description of what is in fact valid.

<p align="center">
  <img src="./images/consoleExtraPrompt.png">
</p>

Assuming communication is possible and input is valid, the application will attempt to interact with the SWAPI.

Once each call is completed, the output is written to the console. 

Once all calls have been complete, a counter is ouput to indicate the number of records processed.

<p align="center">
  <img alt="Retrieved All Starships img" src="images/retrievedAllStarships.png">
</p>

If communication has not been possible, this is communicated to the user and the are given the option to try and to to escape from the application.

If 'Esc' is chosen, application automatically closes after 3 seconds.

<p align="center">
  <img alt="Auto Exit img" src="images/AutoExit.png">
 <p>acsii art by Ray Brunner</p>
</p>
