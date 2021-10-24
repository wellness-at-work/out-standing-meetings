import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { IMeetingResponse } from '../interfaces/IMeetingResponse';
import { sortAttendants } from '../utils/utils';

const connection = new HubConnectionBuilder().withUrl("/meeting")
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

connection.start();

export default function signalRMiddleware(api: { dispatch: (arg0: { type: string; meetingAttendants: any; allTimeRecord: any; }) => void; }) {


    connection.on('MeetingStats', data => {
        const incomingData: IMeetingResponse = data;
        const attendants = incomingData.participants;
        var allTimeRecord = incomingData.allTimeRecord;
        sortAttendants(attendants);
        sortAttendants(allTimeRecord);
        api.dispatch({ type: 'GET_UPDATED_ATTENDANTS', meetingAttendants: data.participants, allTimeRecord: data.allTimeRecord });
    });

    return  (next: (arg0: any) => any) => (action: { type: any; payload: { message: any; }; }) => {

        switch (action.type) {

            // case Actions.DIRECT_MESSAGE: 
            // {
            //     console.log(`enviando msg: ${action.payload.message}`);
            //     connection.invoke('DirectMessage', action.payload);
            // }
        }
        
        return next(action);
    }
}


