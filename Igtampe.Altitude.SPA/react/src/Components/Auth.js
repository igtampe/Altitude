import React from "react";
import Grid from "@mui/material/Grid"
import Typography from "@mui/material/Typography";
import LoginForm from './Subcomponents/LoginForm'

export default function LoginComponent(props) {
    return (
        <React.Fragment>
            <Typography>
                <Grid container spacing={0} direction="column" alignItems="center" justifyContent="center" style={{ minHeight: '50vh' }}>
                    <Grid item xs={12}>
                        <LoginForm DarkMode={props.DarkMode} setSession={props.setSession}/>
                    </Grid>
                </Grid>
            </Typography>
        </React.Fragment>
    );

}
