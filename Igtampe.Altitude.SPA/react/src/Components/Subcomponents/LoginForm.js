import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import { TextField, Button, Typography, CircularProgress, Container } from '@mui/material';
import Cookies from 'universal-cookie';
import { LogIn, Register } from "../../API/User";
import AlertSnackbar from "../Reusable/AlertSnackbar";

// react.school/material-ui


const cookies = new Cookies();

export default function LoginForm(props) {
    const history = useHistory();

    const [Pin, SetPin] = useState("");
    const [ID, SetID] = useState("");
    const [LoginInProgress, SetLoginInProgress] = useState(false);
    const [ResultOpen, setResultOpen] = useState(false);
    const [Result, setResult] = useState({
        text: "Desconozco",
        severity: "danger"
    });

    const handleIDChange = (event) => { SetID(event.target.value); };
    const handlePinChange = (event) => { SetPin(event.target.value); };

    const SetData = (data) => {
        if (data.error || data.errors) {
            setResult({  severity: "error", text: data.reason ?? "An unknown serverside error occurred" });
            setResultOpen(true)
        } else {
            //We logged in, save a cookie, then let's get the heck out of here
            cookies.set('SessionID', data.sessionID, { path: '/', maxAge: 60 * 60 * 24 }) //The cookie will expire in a day
            history.go();
        }
    }

    const setRegisterError = (data) => {
        setResult({  severity: "error", text: data });
        setResultOpen(true)
    }

    const onRegisterSuccess = () => {

        //If this is triggered, there's been success
        //Login now
        OnLoginButtonClick();

    }

    const OnLoginButtonClick = (event) => { LogIn(SetLoginInProgress,ID,Pin,SetData) }
    const OnRegisterButtonClick = (event) => { Register(SetLoginInProgress,ID,Pin,setRegisterError,onRegisterSuccess) }

    return (
        <React.Fragment>
            <Container style={props.DarkMode ? { backgroundColor: '#444444', padding: '50px' } : { backgroundColor: '#ebebeb', padding: '50px' }}>
                <Typography>
                    <Typography variant="h6"
                        style={{ fontFamily: "Outfit", textAlign: "center" }}>
                        <img src="logobg.png" alt="Logo" width="200" />
                    </Typography>
                    <TextField label="Username" value={ID} onChange={handleIDChange} fullWidth
                        style={{ marginTop: "5px", marginBottom: "5px" }} /><br />
                    <TextField label="Password" value={Pin} type="password" onChange={handlePinChange} fullWidth
                        style={{ marginTop: "5px", marginBottom: "5px" }} /><br />

                    <br />
                </Typography>
                <div style={{ textAlign: 'center' }}>
                    {LoginInProgress ? <CircularProgress /> : <>
                        <Button variant='contained' color='primary' disabled={LoginInProgress} onClick={OnLoginButtonClick}
                            style={{ margin: "10px" }}> Log In </Button>
                        <Button variant='contained' color='secondary' disabled={LoginInProgress} onClick={OnRegisterButtonClick}
                            style={{ margin: "10px" }}> Register </Button>
                            
                        </>}
                </div>
            </Container>

            <AlertSnackbar open={ResultOpen} setOpen={setResultOpen} result={Result}/>

        </React.Fragment>
    );

}
