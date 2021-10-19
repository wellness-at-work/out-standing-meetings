import React from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import Link from '@material-ui/core/Link';
import Box from '@material-ui/core/Box';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import { useHistory } from 'react-router-dom';

function Copyright() {
    return (
        <Typography variant="body2" color="textSecondary" align="center">
            {'Copyright © '}
            <Link color="inherit" href="https://github.com/wellness-at-work/out-standing-meetings">
                Wellness at Work
            </Link>{' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}

const useStyles = makeStyles((theme) => ({
    paper: {
        marginTop: theme.spacing(8),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    avatar: {
        margin: theme.spacing(1),
        backgroundColor: theme.palette.secondary.main,
    },
    form: {
        width: '100%', // Fix IE 11 issue.
        marginTop: theme.spacing(1),
    },
    submit: {
        margin: theme.spacing(3, 0, 2),
    },
}));

const SignInControl = () => {
    const classes = useStyles();
    const history = useHistory();
    const handleSubmit = (event: { preventDefault: () => void; }) => {
        history.push("/ranking");
        event.preventDefault();
    }
    return <Container component="main" maxWidth="xs">
        <CssBaseline />
        <div className={classes.paper}>

            <Typography component="h5" variant="h5">
                Outstanding Meetings
            </Typography>
            <Typography component="h6" variant="h6">
                Promoting wellness during office meetings.
            </Typography>
            <Avatar className={classes.avatar}>
                <LockOutlinedIcon />
            </Avatar>
            <Typography component="h3" variant="h3">
                Sign in
            </Typography>
            <form className={classes.form} onSubmit={handleSubmit}>
                <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="code"
                    label="Organization Code"
                    name="code"
                    autoFocus
                />
                <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    color="primary"
                    className={classes.submit}
                >
                    Sign In
                </Button>
            </form>
        </div>
        <Box mt={8}>
            <Copyright />
        </Box>
    </Container>;
}
export default class SignIn extends React.PureComponent<{}>  {

    public render() {

        return (
            <div>
                <SignInControl />
            </div>
        );
    }
}