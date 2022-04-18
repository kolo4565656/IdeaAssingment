import * as React from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import Card from '@mui/material/Card';
import CardActionArea from '@mui/material/CardActionArea';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import { Badge, Button, Chip, Rating } from '@mui/material';
import { Category } from 'models';
import { ThumbDown, ThumbUp, Download } from '@mui/icons-material';

interface FeaturedPostProps {
  modifiedDate?: string;
  createdDate?: string;
  description?: string;
  image?: string;
  imageLabel?: string;
  title?: string;
  creator?: string;
  rating?: number;
  categories?: Category[];
  commentCount?: number;
  dislikeCount?: number;
  likeCount?: number;
  attach?: boolean;
  postId?: string;
}

export default function FeaturedPost(props: FeaturedPostProps) {
  const {
    postId,
    modifiedDate,
    createdDate,
    description,
    image,
    imageLabel,
    title,
    creator,
    rating,
    categories,
    commentCount,
    dislikeCount,
    likeCount,
    attach,
  } = props;

  return (
    <div style={{ position: 'relative' }}>
      <CardActionArea
        style={{ pointerEvents: 'none', padding: '0 72px', position: 'relative' }}
        component="a"
        href="#"
      >
        <Card sx={{ display: 'flex' }}>
          <CardContent sx={{ flex: 1, display: 'flex', flexFlow: 'wrap', alignItems: 'center' }}>
            <Typography
              style={{ marginBottom: '24px', width: '100%' }}
              textAlign={'center'}
              component="h2"
              variant="h5"
            >
              {title}
            </Typography>
            <Typography
              component="span"
              style={{ fontStyle: 'italic', width: '100%' }}
              variant="subtitle1"
              color="text.secondary"
            >
              Modified date: {modifiedDate}
            </Typography>
            <Typography
              component="span"
              style={{ fontStyle: 'italic', width: '100%' }}
              variant="subtitle1"
              color="text.secondary"
            >
              Created date: {createdDate}
            </Typography>
            <Typography
              component="span"
              variant="subtitle1"
              style={{ width: '100%', color: 'gray', margin: '8px 0' }}
            >
              <div
                style={{
                  fontWeight: 'bold',
                  marginRight: '5px',
                  display: 'inline-block',
                }}
              >
                Description:
              </div>
              <div>{description}</div>
            </Typography>
            <Typography component="span" variant="subtitle1" style={{ width: '100%' }}>
              <div
                style={{
                  fontWeight: 'bold',
                  marginRight: '5px',
                  display: 'inline-block',
                }}
              >
                Creator:
              </div>
              {creator}
            </Typography>
            <Typography style={{ display: 'flex', width: '100%' }} component="span">
              <div style={{ fontWeight: 'bold', marginRight: '5px' }}>Rating:</div>
              <Rating name="read-only" value={rating} readOnly precision={0.5} />
              <div style={{ marginLeft: '5px' }}>({commentCount})</div>
            </Typography>
            <Typography
              component="span"
              style={{ marginTop: '12px', display: 'flex', alignItems: 'center' }}
            >
              <div style={{ fontWeight: 'bold', marginRight: '10px' }}>Statistic:</div>
              <Button
                style={{ borderRadius: '25%/50%', marginRight: '15px', backgroundColor: '#00cc99' }}
                variant="contained"
                startIcon={<ThumbUp style={{ marginTop: '-7px', color: 'white' }} />}
              >
                {likeCount}
              </Button>
              <Button
                style={{ borderRadius: '25%/50%', backgroundColor: '#990000' }}
                variant="contained"
                startIcon={<ThumbDown color="error" style={{ color: 'white' }} />}
              >
                {dislikeCount}
              </Button>
            </Typography>
            <Typography
              component="span"
              style={{ display: 'flex', width: '100%', marginTop: '20px' }}
            >
              <div
                style={{
                  fontWeight: 'bold',
                  marginRight: '5px',
                  display: 'flex',
                  alignItems: 'center',
                }}
              >
                Categories:
              </div>
              {Array.isArray(categories) &&
                categories.map((x) => (
                  <Chip
                    key={x.id}
                    style={{ marginLeft: '5px', marginRight: '5px', marginBottom: '5px' }}
                    label={`${x.name}`}
                  />
                ))}
            </Typography>
          </CardContent>
          <CardMedia
            component="img"
            sx={{ width: 650, display: { xs: 'none', sm: 'block' } }}
            src={image}
            alt={imageLabel}
          />
        </Card>
      </CardActionArea>
      <Button
        disabled={!attach}
        style={{ position: 'absolute', top: 0, left: '72px' }}
        href={`/Upload/${postId}/attachment.zip`}
        variant="contained"
        startIcon={<Download style={{ color: 'white' }} />}
      >
        Get attachments
      </Button>
    </div>
  );
}
