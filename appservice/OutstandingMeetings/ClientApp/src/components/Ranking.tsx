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
import { secondsToHms } from '../utils/utils';
import { IMeetingParticipant } from '../interfaces/IMeetingParticipant';


type MeetingProps =
    MeetingStore.MeetingState &
    typeof MeetingStore.actionCreators &
    RouteComponentProps<{}>;

const RankingView = (props: any) => {
    const meetingAttendants = props.props.meetingAttendants;
    const allTimeRecord = props.props.allTimeRecord;
    return <div>
        <Grid container spacing={2}>
            <Grid item md={12}>
                <RankingHeader />
            </Grid>
        </Grid>
        <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
                <ScoreCard participant={allTimeRecord[0]} score={1} />
            </Grid>
            <Grid item xs={12} md={4}>
                <ScoreCard participant={allTimeRecord[1]} score={2} />
            </Grid>
            <Grid item xs={12} md={4}>
                <ScoreCard participant={allTimeRecord[2]} score={3} />
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
    public componentDidMount() {
        this.pollParticipants();
    }
    public componentDidUpdate() {
        
    }
    ensureDataFetched() {
       
    }
    pollParticipants() {
        setInterval(()=>{
            this.props.getUpdatedAttendants("");
        }, 5000)
        
    }
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
                <ListItemText primary={att.name} secondary={secondsToHms(att.duration)} />
            </ListItem>
        ))}
        {meetingAttendants.slice(1, 5).map((att: any, idx: number) => (
            <ListItem key={att.id}>
                <ListItemAvatar>
                    <Avatar className={classes.avatar}>
                        {idx + 2}
                    </Avatar>
                </ListItemAvatar>
                <ListItemText primary={att.name} secondary={secondsToHms(att.duration)} />
            </ListItem>
        ))}
        {meetingAttendants.slice(5).map((att: any) => (
            <ListItem key={att.id}>
                <ListItemAvatar>
                    <Avatar>
                        {att.name.substring(0, 1)}
                    </Avatar>
                </ListItemAvatar>
                <ListItemText primary={att.name} secondary={secondsToHms(att.duration)} />
            </ListItem>
        ))}
    </List>;
}

const RankingHeader = () => {
    return <div>
        <Typography variant="h4" component="div" gutterBottom>
            Outstanding Meetings
        </Typography>
        <Typography variant="h6" component="div" gutterBottom>
            Hall of fame record - most outstanding meeting attendants
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
                {participant.participant.name}
            </Typography>
            <Typography>
                {secondsToHms(participant.participant.duration)}
            </Typography>
        </CardContent>
    </Card>;
}


export default connect(
    (state: ApplicationState) => state.Meeting, // Selects which state properties are merged into the component's props
    MeetingStore.actionCreators // Selects which action creators are merged into the component's props
)(Meeting as any);
