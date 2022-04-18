import { ChevronLeft } from '@mui/icons-material';
import { Divider, Typography } from '@mui/material';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { Tooltip } from 'antd';
import postApi from 'api/postApi';
import { Post } from 'models';
import moment from 'moment';
import * as React from 'react';
import { useParams } from 'react-router';
import { Link } from 'react-router-dom';
import CommentIdea from '../components/CommentIdea';
import FeaturedPost from '../components/FeaturedPost';
import Main from '../components/Main';
import Sidebar from '../components/Sidebar';
const theme = createTheme();

export default function DetailPage() {
  const { postId } = useParams<{ postId: string }>();
  const [markRefresh, setMarkRefresh] = React.useState(true);
  const handleRefresh = () => {
    setMarkRefresh(!markRefresh);
  };
  const [post, setPost] = React.useState<Post>({
    id: '',
    name: '',
    created: new Date(),
    createdBy: '',
    creatorName: '',
    modifierName: '',
    description: '',
    commentCount: 0,
    lastModified: new Date(),
    modifiedBy: '',
    totalStar: 0,
    likeCount: 0,
    dislikeCount: 0,
    content: '',
    isAttachs: false,
    categories: [],
  });
  React.useEffect(() => {
    (async () => {
      try {
        if (postId) {
          const data: Post = await postApi.getById(postId);
          setPost(data);
        }
      } catch (error) {
        console.log('Failed to fetch post details', error);
      }
    })();
  }, [postId, markRefresh]);

  const convertDate = (date: string) => {
    const dateParts = date.split('T');
    const [hour, minute, seccond] = dateParts[1].split(':');
    return `${dateParts[0]} ${hour}:${minute}`;
  };

  return (
    <ThemeProvider theme={theme}>
      <Link to="..">
        <Typography
          borderColor={'blue'}
          variant="caption"
          style={{
            display: 'flex',
            alignItems: 'center',
            fontSize: '24px',
            marginBottom: '24px',
            width: '100%',
          }}
        >
          <ChevronLeft fontSize="medium" /> Go back
        </Typography>
      </Link>
      <FeaturedPost
        title={post.name}
        image={`/Upload/${post.id}/Media/interface.png`}
        modifiedDate={convertDate(post.lastModified.toString())}
        createdDate={convertDate(post.created.toString())}
        description={post.description}
        creator={post.creatorName}
        rating={post.totalStar / post.commentCount}
        categories={post.categories}
        commentCount={post.commentCount}
        likeCount={post.likeCount}
        dislikeCount={post.dislikeCount}
        attach={post.isAttachs}
        postId={postId}
      />
      <Grid container spacing={5} sx={{ mt: 3 }}>
        <Main title="CONTENT" posts={post.content} />
        <Sidebar markRefresh={markRefresh} handleRefresh={handleRefresh} />
      </Grid>
      <Divider style={{ marginTop: '36px' }}>RATING DETAILS ({post.commentCount})</Divider>
      <div style={{ display: 'flex' }}>
        <CommentIdea markRefresh={markRefresh} postId={postId} />
      </div>
    </ThemeProvider>
  );
}
