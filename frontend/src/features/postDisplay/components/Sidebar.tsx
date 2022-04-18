import { Comment } from '@mui/icons-material';
import AssignmentTurnedInIcon from '@mui/icons-material/AssignmentTurnedIn';
import { Button, Rating, TextField } from '@mui/material';
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import { Collapse, Select } from 'antd';
import postApi from 'api/postApi';
import { CommentResponse } from 'models';
import * as React from 'react';
import { useParams } from 'react-router';
import { Edit, Cancel, DeleteForever, SaveAs, ThumbUp, ThumbDown } from '@mui/icons-material';

const { Option } = Select;
const { Panel } = Collapse;

interface SidebarProps {
  markRefresh?: boolean;
  handleRefresh?: () => void;
}

export default function Sidebar(props: SidebarProps) {
  const { handleRefresh, markRefresh } = props;
  const [hasCOmment, setHasComment] = React.useState(false);
  const [isEdit, setIsEdit] = React.useState(false);
  const [show, setShow] = React.useState('');
  const [personal, setPersonal] = React.useState<CommentResponse>({
    id: '',
    userId: '',
    name: '',
    content: '',
    rating: null,
    createdDate: new Date(),
    updatedDate: new Date(),
    userFullName: '',
    subComments: null,
  });
  const [rating, setRating] = React.useState(null);
  const [comment, setComment] = React.useState('');
  const [commentType, setCommentType] = React.useState('Like');
  const { postId } = useParams<{ postId: string }>();

  const handleReset = () => {
    setHasComment(false);
    setPersonal({
      id: '',
      userId: '',
      name: '',
      content: '',
      rating: null,
      createdDate: new Date(),
      updatedDate: new Date(),
      userFullName: '',
      subComments: null,
    });
    setRating(null);
    setComment('');
    setCommentType('Like');
    setIsEdit(false);
  };

  const handleHardReset = () => {
    handleReset();
    handleRefresh();
  };

  const isDirty = () => {
    return rating != personal.rating || comment != personal.content || commentType != personal.name;
  };

  React.useEffect(() => {
    (async () => {
      await postApi.getCommentByUserAndPost(postId).then((e) => {
        if (e) {
          setHasComment(true);
          setPersonal(e);
          setRating(e.rating);
          setComment(e.content);
          setCommentType(e.name);
        } else {
          handleReset();
        }
      });
    })();
  }, [postId, markRefresh]);

  return (
    <Grid item xs={12} md={3}>
      {hasCOmment ? (
        <>
          {isEdit ? (
            <>
              <TextField
                fullWidth
                multiline
                onChange={(e) => setComment(e.target.value)}
                value={comment}
                minRows={4}
                style={{ display: 'block', marginTop: '8px' }}
                label="Comment"
              />
              <div
                style={{
                  display: 'flex',
                  alignItems: 'center',
                  marginTop: '16px',
                  fontWeight: 'bold',
                }}
              >
                Vote:{' '}
                <Rating
                  style={{ marginBottom: '5px' }}
                  value={rating}
                  onChange={(e, value) => setRating(value)}
                />
                <Select
                  value={commentType}
                  style={{
                    marginLeft: 'auto',
                    width: '114px',
                    fontWeight: 'bold',
                  }}
                  onChange={(e) => setCommentType(e)}
                >
                  <Option style={{ fontWeight: 'bold' }} value="Like">
                    Like <ThumbUp style={{ marginBottom: '-2px', paddingTop: '5px' }} />
                  </Option>
                  <Option style={{ fontWeight: 'bold' }} value="Dislike">
                    Dislike <ThumbDown style={{ marginBottom: '-6px', paddingTop: '5px' }} />
                  </Option>
                </Select>
              </div>
            </>
          ) : (
            <>
              <Paper elevation={0} sx={{ p: 2, bgcolor: 'grey.200' }}>
                <Typography component="span" variant="h6" gutterBottom>
                  <AssignmentTurnedInIcon style={{ marginBottom: '-6px', color: 'green' }} />
                  You rated idea
                </Typography>
                <Typography component="div" variant="subtitle1" style={{ width: '100%' }}>
                  <div
                    style={{
                      marginTop: '5px',
                      fontWeight: 'bold',
                      marginRight: '5px',
                      display: 'inline-block',
                    }}
                  >
                    Type:
                  </div>
                  {personal.name}
                </Typography>
                <Typography
                  component="span"
                  style={{ display: 'flex', width: '100%', marginTop: '8px' }}
                >
                  <div style={{ fontWeight: 'bold', marginRight: '5px' }}>Rating:</div>
                  <Rating name="read-only" value={personal.rating} readOnly />
                </Typography>
              </Paper>
              <Collapse style={{ marginTop: '12px' }} onChange={(e) => setShow(e[0])}>
                <Panel header={show == '1' ? 'Hide your comment' : 'Show your comment'} key="1">
                  <p style={{ fontSize: '18px' }}>{personal.content}</p>
                </Panel>
              </Collapse>
            </>
          )}
          <Button
            onClick={async () => {
              if (isDirty()) {
                await postApi
                  .updateComment(
                    {
                      content: comment,
                      name: commentType,
                      rating: rating,
                    },
                    postId,
                    personal.id
                  )
                  .then(() => {
                    handleHardReset();
                  });
              } else {
                setIsEdit(!isEdit);
              }
            }}
            style={{ marginTop: '5px', width: '47.5%', marginRight: '2.5%' }}
            variant="contained"
            color={isEdit ? (isDirty() ? 'primary' : 'error') : 'success'}
            startIcon={isEdit ? isDirty() ? <SaveAs /> : <Cancel /> : <Edit />}
          >
            {isEdit ? (isDirty() ? 'Save' : 'Cancel') : 'Edit'}
          </Button>
          <Button
            onClick={async () => {
              await postApi.deleteComment(postId, personal.id).then(() => {
                handleHardReset();
              });
            }}
            style={{ marginTop: '5px', width: '47.5%', marginLeft: '2.5%' }}
            variant="contained"
            color="error"
            startIcon={<DeleteForever />}
          >
            Delete
          </Button>
        </>
      ) : (
        <>
          <Paper elevation={0} sx={{ p: 2, bgcolor: 'grey.200' }}>
            <Typography variant="h6" gutterBottom>
              Rate this idea !
            </Typography>
            <Typography>You are not rate this idea yet.</Typography>
          </Paper>

          <TextField
            fullWidth
            multiline
            onChange={(e) => setComment(e.target.value)}
            value={comment}
            minRows={4}
            style={{ display: 'block', marginTop: '8px' }}
            label="Comment"
          />
          <div
            style={{
              display: 'flex',
              alignItems: 'center',
              marginTop: '16px',
              fontWeight: 'bold',
            }}
          >
            Vote:{' '}
            <Rating
              style={{ marginBottom: '5px' }}
              value={rating}
              onChange={(e, value) => setRating(value)}
            />
            <Select
              value={commentType}
              style={{
                marginLeft: 'auto',
                width: '114px',
                fontWeight: 'bold',
              }}
              onChange={(e) => setCommentType(e)}
            >
              <Option style={{ fontWeight: 'bold' }} value="Like">
                Like <ThumbUp style={{ marginBottom: '-2px', paddingTop: '5px' }} />
              </Option>
              <Option style={{ fontWeight: 'bold' }} value="Dislike">
                Dislike <ThumbDown style={{ marginBottom: '-6px', paddingTop: '5px' }} />
              </Option>
            </Select>
          </div>

          <Button
            fullWidth
            style={{ marginTop: '16px' }}
            variant="outlined"
            color={'primary'}
            startIcon={<Comment />}
            disabled={comment == '' || rating == null || rating == 0}
            onClick={async () => {
              await postApi
                .createComment(
                  {
                    name: commentType,
                    content: comment,
                    rating: rating,
                  },
                  postId
                )
                .then(() => {
                  handleHardReset();
                });
            }}
          >
            Add a comment
          </Button>
        </>
      )}
    </Grid>
  );
}
