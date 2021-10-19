import { IMeetingParticipant } from "../interfaces/IMeetingParticipant";

export const secondsToHms = (d: number) => {
    var h = Math.floor(d / 3600);
    var m = Math.floor(d % 3600 / 60);
    var s = Math.floor(d % 3600 % 60);

    var hDisplay = h > 0 ? h + (h === 1 ? " hour " : " hours ") : "";
    var mDisplay = m > 0 ? m + (m === 1 ? " minute " : " minutes ") : "";
    var sDisplay = s > 0 ? s + (s === 1 ? " second" : " seconds") : "";
    return "Standing : " + hDisplay + mDisplay + sDisplay;
}

export const sortAttendants = (data :IMeetingParticipant[]) => {

    data.sort((a, b) => {
        if (a.duration === b.duration) {
            return a.name.localeCompare(b.name);
        }
        else {
            return b.duration - a.duration;
        }
    });
}