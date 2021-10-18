import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as MeetingStore from '../store/Meeting';
import { Card, CardContent, CardMedia, Grid, ListItemAvatar, Theme, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Avatar from '@material-ui/core/Avatar';
import Whatshot from '@material-ui/icons/Whatshot';
import { IMeetingParticipant } from '../interfaces/IMeetingParticipant';
import { secondsToHms } from '../utils/utils';


type MeetingProps =
    MeetingStore.MeetingState &
    typeof MeetingStore.actionCreators &
    RouteComponentProps<{}>;

const RankingView = (props: any) => {
    const meetingAttendants = props.props.meetingAttendants;
    return <div>
        <Grid container spacing={2}>
            <Grid item md={12}>
                <RankingHeader />
            </Grid>
        </Grid>
        <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
                <ScoreCard participant={meetingAttendants[0]} score={1} />
            </Grid>
            <Grid item xs={12} md={4}>
                <ScoreCard participant={meetingAttendants[1]} score={2} />
            </Grid>
            <Grid item xs={12} md={4}>
                <ScoreCard participant={meetingAttendants[2]} score={3} />
            </Grid>
        </Grid>
        <Grid container spacing={2}>
            <Grid item md={12}>
                <Typography variant="h6" component="div" gutterBottom>
                    {new Date().getMonth() + 1}/{new Date().getDate()}/{new Date().getFullYear()} top outstanding meeting attendants
                </Typography>
            </Grid>
            <Grid item md={12}>
                <RankingList meetingAttendants={meetingAttendants} />
            </Grid>
        </Grid>
    </div>;
}
class Meeting extends React.PureComponent<MeetingProps> {
    public render() {
        return (
            <RankingView props={this.props} />
        );
    }
};

const useStyles = makeStyles((theme: Theme) => (
    {
        root: {
            width: '100%',
            backgroundColor: theme.palette.background.default,
            flex: "1 0 auto"
        },
        avatar: {
            backgroundColor: theme.palette.secondary.main,
        },
    }));


const RankingList = (meetingAttendants: any) => {
    const classes = useStyles();
    meetingAttendants = meetingAttendants.meetingAttendants;
    return <List className={classes.root}>
        {meetingAttendants.slice(0, 1).map((att: any) => (
            <ListItem key={att.id}>
                <ListItemAvatar>
                    <Avatar className={classes.avatar}>
                        <Whatshot />
                    </Avatar>
                </ListItemAvatar>
                <ListItemText primary={att.Name} secondary={secondsToHms(att.Duration)} />
            </ListItem>
        ))}
        {meetingAttendants.slice(1, 5).map((att: any, idx: number) => (
            <ListItem key={att.id}>
                <ListItemAvatar>
                    <Avatar className={classes.avatar}>
                        {idx + 2}
                    </Avatar>
                </ListItemAvatar>
                <ListItemText primary={att.Name} secondary={secondsToHms(att.Duration)} />
            </ListItem>
        ))}
        {meetingAttendants.slice(5).map((att: any) => (
            <ListItem key={att.id}>
                <ListItemAvatar>
                    <Avatar>
                        {att.Name.substring(0, 1)}
                    </Avatar>
                </ListItemAvatar>
                <ListItemText primary={att.Name} secondary={secondsToHms(att.Duration)} />
            </ListItem>
        ))}
    </List>;
}

const RankingHeader = () => {
    return <div>
        <Typography variant="h4" component="div" gutterBottom>
            Hall of Fame
        </Typography>
        <Typography variant="h6" component="div" gutterBottom>
            Office record for outstanding meeting attendant.
        </Typography>

    </div>;
}

const ScoreCard = (participant: { score: number, participant: IMeetingParticipant }) => {
    let imageLink = "/gold.png";
    if (participant.score === 2) {
        imageLink = "/silver.png";
    } else if (participant.score === 3) {
        imageLink = "/bronze.png";
    }
    return <Card>
        <CardMedia
            component="img"
            height="140"
            image={imageLink}
            alt="Trophy"
        />
        <CardContent>
            <Typography>
                #{participant.score}
            </Typography>
            <Typography variant="h5" component="div">
                {participant.participant.Name}
            </Typography>
            <Typography>
                {secondsToHms(participant.participant.Duration)}
            </Typography>
        </CardContent>
    </Card>;
}


export default connect(
    (state: ApplicationState) => state.Meeting,
    MeetingStore.actionCreators
)(Meeting);
