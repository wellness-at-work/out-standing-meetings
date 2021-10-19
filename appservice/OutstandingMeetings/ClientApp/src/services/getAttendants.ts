import { IMeetingParticipant } from "../interfaces/IMeetingParticipant";


export const getDefaultAttendants = () => {
    let attendantList: IMeetingParticipant[] = [];
    for (let i = 0; i < 5; i++) {
        let attendant: IMeetingParticipant = {
            duration: 5,
            name: "Your name here",
            id: i
        }
        attendantList.push(attendant);
    }

    attendantList.sort((a, b) => {
        if (a.duration === b.duration) {
            return a.name.localeCompare(b.name);
        }
        else {
            return b.duration - a.duration;
        }
    });

    return attendantList;
}