import Ran from 'random-seed';
import {
  BarElement,
  CategoryScale,
  Chart as ChartJS,
  Legend,
  LinearScale,
  Title,
  Tooltip,
} from 'chart.js';
import React, { useEffect, useState } from 'react';
import { Bar } from 'react-chartjs-2';
import { Category, CategoryMultiselect, StatisticParams } from 'models';
import postApi from 'api/postApi';
import { Autocomplete, Button, TextField } from '@mui/material';
import categoryApi from 'api/categoryApi';
import { DatePicker, Space } from 'antd';
import moment from 'moment';
import { Close } from '@mui/icons-material';

const { RangePicker } = DatePicker;

const dateFormat = 'YYYY/MM/DD';
const weekFormat = 'MM/DD';
const monthFormat = 'YYYY/MM';
const yearFormat = 'YYYY';

const rand = Ran.create();

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

export const options = {
  responsive: true,
  scales: {
    y: {
      min: 0,
      max: 20,
    },
  },
  plugins: {
    legend: {
      position: 'top' as const,
    },
    title: {
      display: true,
      text: 'Number of Idea',
    },
  },
};

const labels = [
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December',
];

export function DashBoard() {
  const [categoryOption, setCategoryOption] = React.useState<CategoryMultiselect[]>([]);
  const [open, setOpen] = React.useState(false);
  const [searchCat, setSearchCat] = useState<CategoryMultiselect>({ id: -1, label: '' });
  const [year, setYear] = useState(2022);

  const [params, setParams] = useState<StatisticParams>({
    year: 2022,
    category: null,
  });
  const [statistic, setStatistic] = useState<number[]>([]);

  const data = {
    labels,
    datasets: [
      {
        label: 'Number of idea',
        data: statistic,
        backgroundColor: 'rgba(255, 99, 132, 0.5)',
      },
    ],
  };

  useEffect(() => {
    (async () => {
      await postApi.getPostStatistic(params).then((e) => {
        setStatistic(e);
      });
    })();
  }, [params]);

  useEffect(() => {
    (async () => {
      await categoryApi
        .get({
          pageSize: 1000,
        })
        .then((e) => {
          setCategoryOption(
            e.data.map((x) => {
              return {
                id: x.id,
                label: x.name,
              };
            })
          );
        });
    })();
  }, []);

  return (
    <>
      <div style={{ display: 'flex', alignItems: 'center' }}>
        <DatePicker
          value={moment(year, yearFormat)}
          style={{ marginLeft: '84px' }}
          format={yearFormat}
          picker="year"
          onChange={(e, value) => {
            setYear(parseInt(value));
            setParams({ ...params, year: parseInt(value) });
          }}
        />
        <Autocomplete
          value={searchCat}
          id="combo-box-demo"
          size="small"
          style={{ marginLeft: 'auto' }}
          options={categoryOption}
          disableClearable
          onChange={(event, value) => {
            setSearchCat(value as CategoryMultiselect);
            value && setParams({ ...params, category: (value as CategoryMultiselect).id });
          }}
          sx={{ width: 300 }}
          renderInput={(params) => <TextField {...params} value={searchCat} label="Category" />}
        />
        <Button
          variant="contained"
          color="error"
          style={{ marginLeft: '24px' }}
          onClick={() => {
            setParams({
              year: 2022,
              category: null,
            });
            setSearchCat({ id: -1, label: '' });
            setYear(2022);
          }}
        >
          <Close style={{ fontSize: '18px', marginBottom: '2px' }} />
          Clear
        </Button>
      </div>
      <div style={{ width: '90%', marginLeft: '5%', display: 'flex', justifyContent: 'center' }}>
        <Bar options={options} data={data} />
      </div>
    </>
  );
}
