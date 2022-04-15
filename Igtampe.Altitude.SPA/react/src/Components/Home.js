import { Button, Card, CardContent, Divider, Grid, Typography } from '@mui/material';
import React from 'react';
import { useHistory } from "react-router-dom";

function AdCard(props) {
    return (
        <Card sx={{ minWidth: '100%' }}>
            <CardContent>
                <img src={props.img} width='100%' alt='' style={props.fit ? { height: '200px', objectFit: 'cover' } : {}} />
                <Typography gutterBottom sx={{ textAlign: 'center', marginTop: '10px' }}> <h4>{props.title}</h4> </Typography>
                <Divider />
                <div style={{ minWidth: '100%', maxHeight: '200px', overflowY: 'auto' }} >
                    {props.children}
                </div>
            </CardContent>
        </Card>)
}

export default function Home(props) {
    const history = useHistory();

    const ImageOverlayStyle = { position: 'absolute', bottom: '50%', left: '5%', transform: 'translate(0%,50%)', textShadow: '2px 2px 8px #000000' }

    return (
        <div>

            <div style={{ marginBottom: '15px', position: 'relative' }}>
                <img src='/images/home.jpg' alt='homepage' width='100%' style={{ height: '300px', objectFit: 'cover' }} /><br />
                <div style={ImageOverlayStyle}>
                    <table>
                        <tr>
                            <td rowSpan={2 }><img src='/logo.png' height='200px' alt='Neco Logo' /></td>
                            <td style={{verticalAlign:'bottom', color:'white'}}>
                                <Typography style={{fontSize:'30px'}}><b>New horizons await!</b></Typography>
                            </td>
                        </tr>
                        <tr><td style={{verticalAlign:'top', color:'white'}}>Schedule your next trip with Horizons, and <br/>make sure you don't miss a single one</td></tr>
                    </table>
                </div>
            </div>

            <div style={{ textAlign: 'center', marginTop: '20px' }}>
                <Button onClick={() => { history.push("/Login") }} variant={'contained'}> Start Planning</Button>
            </div>
            <br />
            <Grid container style={{ minWidth: '100%' }} spacing={2}>
                <Grid item xs={props.Vertical ? 12 : 4}>
                    <AdCard title='Simple' img='/Screenshots/NecoShot1.png' fit={!props.Vertical}>
                        Plan out your trips broken down into Days and Events. Move them around as you please. It's always good to dream, and change your mind.
                    </AdCard>
                </Grid>

                <Grid item xs={props.Vertical ? 12 : 4}>
                    <AdCard title='Sharable' img='/Screenshots/NecoShot4.png' fit={!props.Vertical}>
                        Share your trips with friends and family, or make your trip public so that anyone with the link can see it. Hopefully it won't make them too jelaous.
                    </AdCard>
                </Grid>

                <Grid item xs={props.Vertical ? 12 : 4}>
                    <AdCard title='Take it Anywhere' img='/Screenshots/NecoShot2.png' fit={!props.Vertical}>
                        Horizons makes it easy to build your plan on platform, then export it and access it anywhere, like your calendar, or as a printable PDF.
                    </AdCard>
                </Grid>

            </Grid>
        </div>
    );
}
