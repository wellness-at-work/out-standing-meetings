import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import { IMeetingResponse } from '../interfaces/IMeetingResponse';
import { IMeetingParticipant } from '../interfaces/IMeetingParticipant';
import { getDefaultAttendants } from '../services/getAttendants';
import { sortAttendants } from '../utils/utils';


export interface MeetingState {
    meetingAttendants: IMeetingParticipant[],
    allTimeRecord: IMeetingParticipant[]
}

export interface GetDefaultParticipantsAction { type: 'GET_DEFAULT_ATTENDANTS', meetingAttendants: IMeetingParticipant[], allTimeRecord: IMeetingParticipant[] }
export interface GetUpdatedParticipantsAction { type: 'GET_UPDATED_ATTENDANTS', meetingAttendants: IMeetingParticipant[], allTimeRecord: IMeetingParticipant[] }
export interface GetUAllTimeRecordAction { type: 'GET_ALL_TIME_RECORD', meetingAttendants: IMeetingParticipant[], allTimeRecord: IMeetingParticipant[] }

export type KnownAction = GetDefaultParticipantsAction | GetUpdatedParticipantsAction | GetUAllTimeRecordAction;

export const actionCreators = {
    getDefaultAttendants: () => ({ type: 'GET_DEFAULT_ATTENDANTS' } as GetDefaultParticipantsAction),
    getAllTimeRecord: () => ({ type: 'GET_ALL_TIME_RECORD' } as GetUAllTimeRecordAction),
    getUpdatedAttendants: (orgCode: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.Meeting) {
            fetch(`/Meeting/Participants?orgCode=${orgCode}`)
                .then(response => response.json() as Promise<IMeetingResponse>)
                .then(data => {
                    sortAttendants(data.allTimeRecord);
                    sortAttendants(data.participants);
                    dispatch({ type: 'GET_UPDATED_ATTENDANTS', meetingAttendants: data.participants, allTimeRecord: data.allTimeRecord });
        });
    }
}
};


export const reducer: Reducer<MeetingState> = (state: MeetingState | undefined, incomingAction: Action): MeetingState => {
    let attendants = getDefaultAttendants();

    if (state === undefined) {
        return { meetingAttendants: attendants, allTimeRecord: attendants.slice(0, 3) };
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'GET_DEFAULT_ATTENDANTS':
            {
                return { meetingAttendants: attendants, allTimeRecord: attendants.slice(0, 3) };
            }
        case 'GET_UPDATED_ATTENDANTS':
            {
                attendants = (incomingAction as any).meetingAttendants;
                var allTimeRecord = (incomingAction as any).allTimeRecord;
                return { meetingAttendants: attendants, allTimeRecord: allTimeRecord };
            }
        default:
            return state;
    }
};
