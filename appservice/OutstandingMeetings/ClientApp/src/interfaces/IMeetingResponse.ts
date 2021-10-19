import { IMeetingParticipant } from "./IMeetingParticipant";

export interface IMeetingResponse {
    participants : IMeetingParticipant[],
    allTimeRecord: IMeetingParticipant[]
}