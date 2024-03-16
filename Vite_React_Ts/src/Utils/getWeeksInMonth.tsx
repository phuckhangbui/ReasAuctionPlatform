import dayjs from "dayjs";

export const getWeeksInMonth = (
  year: dayjs.Dayjs | null,
  month: dayjs.Dayjs | null
) => {
  const weeks = [];
  let yearSeleted = 0;
  let monthSeleted = 0;
  if (year && month) {
    yearSeleted = year.get("year");
    monthSeleted = month.get("month");
  }
  const firstDay = new Date(yearSeleted, monthSeleted, 1);
  const lastDay = new Date(yearSeleted, monthSeleted + 1, 0);

  let currentWeek = [];
  let currentDate = new Date(firstDay);

  // Move to the first day of the week (Sunday)
  currentDate.setDate(currentDate.getDate() - ((currentDate.getDay() + 6) % 7));

  while (currentDate <= lastDay) {
    // Check if the current day belongs to the same month
    if (currentDate.getMonth() === monthSeleted) {
      currentWeek.push(new Date(currentDate));
    }

    currentDate.setDate(currentDate.getDate() + 1);

    // Check if the next day is a Monday and belongs to the same month
    if (currentDate.getDay() === 1 && currentDate.getMonth() === monthSeleted) {
      weeks.push([...currentWeek]);
      currentWeek = [];
    }
  }

  // Add the last week if it's not complete
  if (currentWeek.length > 0) {
    weeks.push([...currentWeek]);
  }

  return weeks;
};
