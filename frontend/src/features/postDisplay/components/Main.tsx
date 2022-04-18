import * as React from 'react';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';
import Markdown from './Markdown';

interface MainProps {
  posts?: string;
  title: string;
}

export default function Main(props: MainProps) {
  const { posts, title } = props;

  return (
    <Grid
      item
      xs={12}
      md={9}
      sx={{
        '& .markdown': {
          py: 3,
        },
      }}
    >
      <Typography style={{ textAlign: 'center' }} variant="h6" gutterBottom>
        {title}
      </Typography>
      <Divider style={{ marginBottom: '24px' }} />
      <div style={{ marginLeft: '24px', marginRight: '24px' }}>
        <Markdown className="markdown">{posts}</Markdown>
      </div>
    </Grid>
  );
}
