import { IMeetingParticipant } from "../interfaces/IMeetingParticipant";

export const getDefaultAttendants = () => {
    let attendantList: IMeetingParticipant[] = [
        {
            id: 1,
            Name: "Kathleen Antonelli",
            Duration: 19000
        },
        {
            id: 2,
            Name: "John Bardeen",
            Duration: 18000
        },
        {
            id: 3,
            Name: "Anita Borg",
            Duration: 19000
        },
        {
            id: 4,
            Name: "Annie Jump Cannon",
            Duration: 20000
        },
        {
            id: 5,
            Name: "Steve Wozniak",
            Duration: 21000
        },
        {
            id: 6,
            Name: "Rosalyn Sussman Yalow",
            Duration: 22000
        },
        {
            id: 7,
            Name: "Nikolay Yegorovich Zhukovsky"
            ,
            Duration: 23000
        }];

        attendantList.sort((a, b) => {
            if (a.Duration === b.Duration) {
                return a.Name.localeCompare(b.Name);
            }
            else {
                return b.Duration - a.Duration;
            }
        });

        return attendantList;
}