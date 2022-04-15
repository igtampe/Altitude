import React, { useState } from "react";
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, CircularProgress, TextField } from "@mui/material";
import Cookies from 'universal-cookie';
import { APIURL, GenerateJSONPut } from "../../../API/common";
import AlertSnackbar from "../../Reusable/AlertSnackbar";

const cookies = new Cookies();

export default function PasswordChangeButton(props) {

    const [OldPassword, setOldPass] = useState("");
    const [NewPassword, setNewPass] = useState("");
    const [PassMatch, setPassMatch] = useState(true)

    const [PassOpen, setPassOpen] = useState(false);
    const [InProgress, setInProgress] = useState(false);

    const [result, setResult] = useState({ severity: "success", text: "idk" })
    const [SnackOpen, setSnackOpen] = useState(false);

    const handleChangePass = (event) => {
        if (OldPassword === "" || NewPassword === "") {
            setResult({
                severity: "danger",
                text: "Old and new passwords must not be empty"
            })
            setSnackOpen(true);
            return;
        }

        if (!PassMatch) {
            setResult({
                severity: "danger",
                text: "Passwords do not match"
            })
            setSnackOpen(true);
            return;
        }

        setInProgress(true);

        //Grab the ID and pin and create a tiny itty bitty object
        const requestOptions = GenerateJSONPut(cookies.get("SessionID"),{
            "current": OldPassword,
            "new": NewPassword
        })
        
        fetch(APIURL + "/API/Users", requestOptions)
            .then(response => {
                setInProgress(false);
                return response.text()
            }).then(data => {
                if (data !== "") {
                    setResult({
                        severity: "danger",
                        text: data
                    })
                    setSnackOpen(true);
                } else {
                    //s u c c e s s
                    setResult({
                        severity: "success",
                        text: "Password changed successfully"
                    })
                    setSnackOpen(true);
                    setPassOpen(false);
                    setOldPass("");
                    setNewPass("");

                }
            })

    }

    return (
        <React.Fragment>

            <Button onClick={() => { setPassOpen(true) }} color='secondary'> Change Password </Button>

            <Dialog maxWidth="xs" open={PassOpen} onClose={() => setPassOpen(false)}>
                <DialogTitle>Change Password</DialogTitle>
                <DialogContent>
                    <TextField label="Old Password" value={OldPassword} type="password" disabled={InProgress} onChange={(event) => setOldPass(event.target.value)} fullWidth
                        style={{ marginTop: "5px", marginBottom: "5px" }} /><br />
                    <TextField label="New Password" value={NewPassword} type="password" disabled={InProgress} onChange={(event) => setNewPass(event.target.value)} fullWidth
                        style={{ marginTop: "5px", marginBottom: "5px" }} /><br />
                    <TextField label="Confirm Password" type="password" disabled={InProgress} error={!PassMatch} helperText={PassMatch ? "" : "Passwords do not match"} onChange={(event) => setPassMatch(event.target.value === NewPassword)} fullWidth
                        style={{ marginTop: "5px", marginBottom: "5px" }} /><br />
                </DialogContent>
                <DialogActions>
                    {InProgress ? <CircularProgress size="20px" /> : <>
                        <Button onClick={handleChangePass}>OK</Button>
                        <Button onClick={() => setPassOpen(false)}>Cancel</Button>
                    </>
                    }
                </DialogActions>
                <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result} />
            </Dialog>
        </React.Fragment>
    );

}
