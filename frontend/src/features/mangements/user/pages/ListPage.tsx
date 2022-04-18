import { AddBox, Close, Delete, Edit, Search } from '@mui/icons-material';
import { Button, FormControl, IconButton, InputLabel, OutlinedInput } from '@mui/material';
import Modal from '@mui/material/Modal';
import Pagination from '@mui/material/Pagination';
import Stack from '@mui/material/Stack';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import userApi from 'api/userApi';
import RegisterUserForm from 'components/Common/Form/RegisterUserForm';
import { ListParams, User } from 'models';
import React, { useEffect, useState } from 'react';
import { Modal as Modala, Button as Butonla } from 'antd';

const initParams: ListParams = {
  keyword: '',
  pageIndex: 0,
  pageSize: 5,
  sort: '',
};

export default function ListPage() {
  //Modal
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [userDel, setDel] = useState('');

  const showModal = (id: string) => {
    setDel(id);
    setIsModalVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
  };
  //End modal
  const [, setLoading] = useState(false);
  const [openModal, setModal] = useState(false);
  const [edit, setEdit] = useState('');
  const [data, setData] = useState<User[]>([]);
  const [pagination, setPagination] = useState<ListParams>(initParams);
  const [total, setTotal] = useState(1);
  const [asc, setAsc] = useState(true);
  const [sort, setSort] = useState('');
  const [key, setKey] = useState('');
  const [page, setPage] = useState(1);

  const closeModal = () => {
    setModal(false);
    setEdit('');
  };

  useEffect(() => {
    loadData();
  }, [pagination]);

  const loadData = async () => {
    setLoading(true);
    await userApi
      .get(pagination)
      .then((res) => {
        setLoading(false);
        if (Array.isArray(res.data)) {
          setData(res.data);
          setTotal(res.paging.totalPages);
        } else {
          throw new Error('Response users data is not an array');
        }
      })
      .catch((error) => {
        setLoading(false);
        console.log(error);
      });
  };

  const handleOk = async (id: string) => {
    setLoading(true);
    await userApi
      .delete(id)
      .then(() => {
        setIsModalVisible(false);
        loadData();
        setLoading(false);
      })
      .catch((error) => {
        setIsModalVisible(false);
        setLoading(false);
        console.log(error);
      });
  };

  const registerUser = async (formValues: User, dirtyFields: any[]) => {
    if (edit) {
      await userApi
        .edit(
          dirtyFields.reduce((x, y) => {
            return {
              ...x,
              [`${y.field}`]: `${y.value}`,
            };
          }, {}),
          edit
        )
        .then(() => {
          setPagination({ ...pagination });
          closeModal();
        });
    } else {
      await userApi.register(formValues).then(() => {
        setPagination({ ...pagination });
        closeModal();
      });
    }
  };

  const headerName: string = sort
    ? `No. (Sort: ${sort} - ${!asc ? 'Ascending' : 'Descending'})`
    : 'No. (Sort: N/A)';

  const columnsModel: GridColDef[] = [
    {
      field: 'number',
      headerName: headerName,
      headerAlign: 'center',
      width: 250,
      align: 'center',
      renderCell: (value) => <span>{value.api.getRowIndex(value.id) + 1}</span>,
      editable: false,
      sortable: false,
    },
    {
      field: 'userName',
      headerName: 'User Name',
      headerAlign: 'center',
      headerClassName: 'header-grid',
      width: 250,
      type: 'string',
      align: 'center',
      disableColumnMenu: true,
      editable: false,
      sortable: false,
    },
    {
      field: 'firstName',
      headerName: 'First Name',
      headerAlign: 'center',
      headerClassName: 'header-grid',
      width: 250,
      type: 'string',
      align: 'center',
      disableColumnMenu: true,
      editable: false,
      sortable: false,
    },
    {
      field: 'lastName',
      headerName: 'Last Name',
      headerAlign: 'center',
      headerClassName: 'header-grid',
      width: 250,
      type: 'string',
      align: 'center',
      disableColumnMenu: true,
      editable: false,
      sortable: false,
    },
    {
      field: 'email',
      headerName: 'Email',
      headerAlign: 'center',
      headerClassName: 'header-grid',
      width: 250,
      type: 'string',
      align: 'center',
      disableColumnMenu: true,
      editable: false,
      sortable: false,
    },
    {
      field: 'actions',
      headerName: 'Action',
      width: 250,
      type: 'actions',
      align: 'center',
      editable: false,
      sortable: false,
      disableColumnMenu: true,
      renderCell: (data) => (
        <>
          <IconButton aria-label="delete" onClick={() => showModal(data.row.id)}>
            <Delete />
          </IconButton>
          <IconButton
            aria-label="edit"
            onClick={() => {
              setEdit(data.row.id);
              setModal(true);
            }}
          >
            <Edit />
          </IconButton>
        </>
      ),
    },
  ];

  return (
    <>
      <div style={{ display: 'flex' }}>
        <IconButton aria-label="add" size="medium" onClick={() => setModal(true)}>
          <AddBox style={{ fontSize: '53px' }} />
        </IconButton>
        <FormControl style={{ width: '500px', paddingTop: '15px' }} variant="outlined" size="small">
          <InputLabel style={{ paddingTop: '15px' }} htmlFor="searchByName">
            Search (all field available)
          </InputLabel>
          <OutlinedInput
            id="searchByName"
            style={{ width: '500px' }}
            label="Search (all field available)"
            endAdornment={<Search />}
            onChange={(e) => {
              setKey(e.target.value);
            }}
            value={key}
          />
        </FormControl>
        <Button
          style={{
            display: 'inline-block',
            border: '1px solid black',
            marginLeft: '5px',
            maxHeight: '40px',
            marginTop: '15px',
          }}
          onClick={() => {
            setPagination({ ...pagination, keyword: key });
          }}
        >
          Submit
        </Button>

        <Button
          variant="contained"
          color="error"
          style={{
            display: 'inline-block',
            marginLeft: 'auto',
            maxHeight: '40px',
            marginTop: '15px',
          }}
          onClick={() => {
            setKey('');
            setSort('');
            setPage(1);
            setPagination(initParams);
          }}
        >
          <Close style={{ fontSize: '18px', marginBottom: '-4px' }} />
          Clear
        </Button>
      </div>

      <DataGrid
        rows={data}
        columns={columnsModel}
        columnVisibilityModel={{}}
        cell--textCenter
        hideFooterSelectedRowCount={true}
        hideFooterPagination={true}
        style={{ height: '75vh' }}
        onColumnHeaderClick={(e) => {
          console.log(e);
          const field: string = e.field;
          if (!(field == 'actions' || field == 'number')) {
            const formatField = e.field.charAt(0).toUpperCase() + e.field.slice(1);
            const order = asc ? '' : ' DESC';
            setPagination({
              ...pagination,
              sort: formatField + order,
            });
            setAsc(!asc);
            setSort(formatField.replace(/([a-z0-9])([A-Z])/g, '$1 $2'));
          }
        }}
      />
      <Stack paddingTop={'24px'} spacing={2} style={{ float: 'right' }}>
        <Pagination
          page={page}
          onChange={(e, value) => {
            setPage(value);
            setPagination({
              ...pagination,
              pageIndex: value - 1,
            });
          }}
          count={total}
          variant="outlined"
          color="primary"
        />
      </Stack>
      <Modal
        open={openModal}
        onClose={closeModal}
        style={{
          display: 'flex',
          justifyContent: 'center',
        }}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <>
          <Button
            style={{
              marginTop: '72px',
              marginBottom: '72px',
            }}
            type="submit"
            variant="contained"
            color="secondary"
            onClick={closeModal}
          >
            &nbsp;Back
          </Button>
          <RegisterUserForm userId={edit} onSubmit={registerUser} />
        </>
      </Modal>
      <Modala
        title="Delete"
        visible={isModalVisible}
        onOk={() => handleOk(userDel)}
        onCancel={handleCancel}
      >
        <p>Continue to process delete?</p>
      </Modala>
    </>
  );
}
