# App Service #

App service hosts the website that displays the standing status of meeting participants. 

## Current State ##

Website using pulling strategy to capture the updated status of meeting participants from an Azure function end point.

## Future State ##

The website will support web sockets to get instant update status of meeting participants using various data sources such as Ms Bot in Teams, contact sensors from IoT Hub.