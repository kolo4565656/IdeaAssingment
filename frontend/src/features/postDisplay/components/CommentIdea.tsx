import { Comment, List, Pagination, Tooltip } from 'antd';
import postApi from 'api/postApi';
import { ApiResponse, CommentResponse, ListParams } from 'models';
import moment from 'moment';
import * as React from 'react';
import date from 'date-and-time';
import { format } from 'timeago.js';
import { Rating } from '@mui/material';

interface CommentProbs {
  markRefresh?: boolean;
  postId?: string;
}

export default function CommentIdea(props: CommentProbs) {
  const { postId, markRefresh } = props;
  const [commemt, setCommemt] = React.useState<ApiResponse<CommentResponse>>({
    paging: {
      pageIndex: 0,
      pageSize: 5,
      totalItemsCount: 0,
      totalPages: 0,
    },
    data: [],
  });
  const [params, setParams] = React.useState<ListParams>({
    pageIndex: 0,
    pageSize: 5,
    sort: '',
    keyword: '',
  });

  React.useEffect(() => {
    (async () => {
      await postApi.getCommentByPost(postId, params).then((e) => {
        setCommemt(e);
      });
    })();
  }, [params, markRefresh]);

  return (
    <div style={{ position: 'relative', width: '100%' }}>
      <List
        style={{ marginLeft: '48px', marginRight: '48px' }}
        className="comment-list"
        itemLayout="horizontal"
        dataSource={commemt.data}
        renderItem={(item) => (
          <li style={{ position: 'relative' }}>
            <div style={{ position: 'absolute', right: 0, top: '16px' }}>
              <div style={{ fontSize: '20px', fontWeight: 'bold' }}>Rating:</div>
              <Rating name="read-only" value={item.rating} readOnly precision={0.5} />
            </div>

            <Comment
              actions={[<span key="comment-list-reply-to-0">Reply to {item.userFullName}</span>]}
              author={item.userFullName}
              avatar={'https://joeschmoe.io/api/v1/random'}
              content={item.content}
              datetime={
                <Tooltip
                  title={moment(item.updatedDate, 'YYYY-MM-DDTHH:mm:ss').format(
                    'YYYY-MM-DD HH:mm:ss'
                  )}
                >
                  <span>
                    {format(
                      moment(item.updatedDate, 'YYYY-MM-DDTHH:mm:ss').format('YYYY-MM-DD HH:mm:ss')
                    )}
                  </span>
                </Tooltip>
              }
            />
          </li>
        )}
      />
      <Pagination
        onChange={(e, value) => setParams({ ...params, pageIndex: e - 1 })}
        style={{ position: 'absolute', right: 0 }}
        current={commemt.paging.pageIndex + 1}
        defaultCurrent={1}
        total={commemt.paging.totalItemsCount}
        pageSize={commemt.paging.pageSize}
      />
    </div>
  );
}
