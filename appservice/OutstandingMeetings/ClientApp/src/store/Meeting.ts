import { Action, Reducer } from 'redux';
import { IMeetingParticipant } from '../interfaces/IMeetingParticipant';
import { getDefaultAttendants } from '../services/getAttendants';


export interface MeetingState {
    count: number;
    meetingAttendants: IMeetingParticipant[];
}

export interface IncrementCountAction { type: 'GET_ATTENDANTS' }

export type KnownAction = IncrementCountAction;

export const actionCreators = {
    increment: () => ({ type: 'GET_ATTENDANTS' } as IncrementCountAction),
};


export const reducer: Reducer<MeetingState> = (state: MeetingState | undefined, incomingAction: Action): MeetingState => {
    if (state === undefined) {
        return { count: 0, meetingAttendants: getDefaultAttendants() };
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'GET_ATTENDANTS':
            {
                return { count: state.count + 1, meetingAttendants: getDefaultAttendants() };
            }
        default:
            return state;
    }
};
