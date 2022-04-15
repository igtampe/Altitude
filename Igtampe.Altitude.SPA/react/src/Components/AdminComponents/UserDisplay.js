import React, { useState } from "react";
import { Box, TextField, IconButton, Table, TableContainer, TableHead, TableRow, TableCell, TableBody, CircularProgress, Paper, Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@mui/material'
import SearchIcon from '@mui/icons-material/Search'
import {Password} from '@mui/icons-material'
import { APIURL, GenerateJSONPut } from "../../API/common";
import AlertSnackbar from "../Reusable/AlertSnackbar";

//This entire component was basically taken straight from Neco
//oh neco, without you, where would I be?

function ResetPassForm(props){

    const [newPass,setNewPass] = useState('')

    const clearForm = () => { setNewPass('') }
    const handleClosing = () => { 
        clearForm();
        props.setOpen(false) 
    }

    const handleOK = () => {

        if (newPass === "") {
            props.setResult({ severity: "danger", text: "New password must not be empty" })
            props.setSnackOpen(true);
            return;
        }

        //Grab the ID and pin and create a tiny itty bitty object
        const requestOptions = GenerateJSONPut(props.Session,{ "new": newPass})
        
        fetch(APIURL + '/API/Users/' + props.user.username + '/reset', requestOptions)
            .then(response => {
                return response.text()
            }).then(data => {
                if (data !== "") {
                    props.setResult({ severity: "danger", text: data})
                    props.setSnackOpen(true);
                } else {
                    //s u c c e s s
                    props.setResult({ severity: "success", text: "Password changed successfully" })
                    props.setSnackOpen(true);
                    handleClosing();
                }
            })



    }


    if(!props.user){return (<></>)}

return(
    <>
    <Dialog maxWidth='xs' fullWidth open={props.open} onClose={handleClosing}>
        <DialogTitle> Reset password for {props.user.username} </DialogTitle>
        <DialogContent>
            <TextField label="ID" value={props.user.username} fullWidth style={{ marginTop: "5px", marginBottom: "5px" }} />
            <TextField label="New Password" value={newPass} onChange={(event)=> setNewPass(event.target.value)} fullWidth type='password'
                                style={{ marginTop: "5px", marginBottom: "5px" }} />
        </DialogContent>
        <DialogActions>
            <Button onClick={handleOK}>Ok</Button>
            <Button onClick={handleClosing}>Cancel</Button>
        </DialogActions>
    </Dialog>

    </>
    )
}

function UserRow(props) {

    //This is basically a component
    const [resetOpen, setResetOpen] = useState(false)

    const [result, setResult] = useState({ severity: "success", text: "idk" })
    const [SnackOpen, setSnackOpen] = useState(false);

    return (
        <>
            <TableRow>
                <TableCell width={'50px'}>
                    <a href={props.U.imageURL === "" ? "/icons/person.png" : props.U.imageURL}>
                        <img alt={'Profile'} src={props.U.imageURL === "" ? "/icons/person.png" : props.U.imageURL} height={'50px'} /></a>
                </TableCell>
                <TableCell>{props.U.username}</TableCell>
                <TableCell width={'70px'}> <IconButton onClick={()=>setResetOpen(true)}> <Password/></IconButton></TableCell>
            </TableRow>

            <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result} />
            <ResetPassForm open={resetOpen} setOpen={setResetOpen} user={props.U} setResult={setResult} setSnackOpen={setSnackOpen} {...props}/>

        </>
    )


}

export default function UserDisplay(props) {

    const [query, setQuery] = useState("");

    const [users, setUsers] = useState(undefined)
    const [loading, setLoading] = useState(false);

    const startSearch = (event) => { setUsers(undefined) }

    //OK now 
    if (!users && !loading) {

        setLoading(true)

        var URL = APIURL + '/API/Users/Dir'
        if (query !== "") { URL = URL + '?Query=' + query }

        fetch(URL).then(response => response.json())
            .then(data => {

                //if there was an error then oops
                if (data.error) { return; }

                setUsers(data)
                setLoading(false)

            })

    }


    return (
        <React.Fragment>
            <Box sx={{ display: 'flex', alignItems: 'flex-end' }}>
                <TextField fullWidth label="Search" value={query} onChange={(event) => { setQuery(event.target.value) }} />
                <IconButton onClick={startSearch} style={{ marginLeft: '10px', marginBottom: '7px' }}><SearchIcon /></IconButton>
            </Box>
            <TableContainer component={Paper} style={{ marginTop: '25px' }}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>PFP</TableCell>
                            <TableCell>Username</TableCell>
                            <TableCell>Reset</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {
                            !users ?
                                <TableRow>
                                    <TableCell colSpan={9} style={{ textAlign: "center" }}><CircularProgress /></TableCell>
                                </TableRow> : <>{

                                    users.length === 0 ?
                                        <TableRow>
                                            <TableCell colSpan={9} style={{ textAlign: "center" }}>No Users found</TableCell>
                                        </TableRow> : <>{users.map(U => <UserRow U={U} setUsers={setUsers} Session={props.Session} />)}</>
                                }
                                </>
                        }
                    </TableBody>
                </Table>
            </TableContainer>

        </React.Fragment>
    );

}
