import React from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Avatar from '@material-ui/core/Avatar';
import Whatshot from '@material-ui/icons/Whatshot';
import { makeStyles } from '@material-ui/core/styles';
import { ListItemAvatar, Theme, Typography } from '@material-ui/core';

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


const secondsToHms = (d: number) => {
    var h = Math.floor(d / 3600);
    var m = Math.floor(d % 3600 / 60);
    var s = Math.floor(d % 3600 % 60);

    var hDisplay = h > 0 ? h + (h === 1 ? " hour " : " hours ") : "";
    var mDisplay = m > 0 ? m + (m === 1 ? " minute " : " minutes ") : "";
    var sDisplay = s > 0 ? s + (s === 1 ? " second" : " seconds") : "";
    return "Standing : " + hDisplay + mDisplay + sDisplay;
}

let attendantList = [
    {
        name: "Kathleen Antonelli",
        duration: 19000
    },
    {
        name: "John Bardeen",
        duration: 18000
    },
    {
        name: "Anita Borg",
        duration: 19000
    },
    {
        name: "Annie Jump Cannon",
        duration: 20000
    },
    {
        name: "Steve Wozniak",
        duration: 21000
    },
    {
        name: "Rosalyn Sussman Yalow"
        ,
        duration: 22000
    },
    {
        name: "Nikolay Yegorovich Zhukovsky"
        ,
        duration: 23000
    }];

attendantList.sort((a, b) => {
    if (a.duration === b.duration) {
        return a.name.localeCompare(b.name);
    }
    else {
        return b.duration - a.duration;
    }
});

const RankingList = () => {
    const classes = useStyles();
    return <List className={classes.root}>
        {attendantList.slice(0, 1).map((att, idx) => (
            <ListItem>
                <ListItemAvatar>
                    <Avatar className={classes.avatar}>
                        <Whatshot />
                    </Avatar>
                </ListItemAvatar>
                <ListItemText primary={att.name} secondary={secondsToHms(att.duration)} />
            </ListItem>
        ))}
        {attendantList.slice(1, 5).map((att, idx) => (
            <ListItem>
                <ListItemAvatar>
                    <Avatar className={classes.avatar}>
                        {idx + 2}
                    </Avatar>
                </ListItemAvatar>
                <ListItemText primary={att.name} secondary={secondsToHms(att.duration)} />
            </ListItem>
        ))}
        {attendantList.slice(5).map((att, idx) => (
            <ListItem>
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
            Outstanding meeting attendants
        </Typography>
        <Typography variant="h6" component="div" gutterBottom>
            {new Date().getMonth()}/{new Date().getDate()}/{new Date().getFullYear()}
        </Typography>
    </div>;
}

export default class Ranking extends React.PureComponent<{}> {
    public state = {
        isOpen: false
    };

    public render() {

        return (
            <div>
                <RankingHeader />
                <RankingList />
            </div>
        );
    }
}
