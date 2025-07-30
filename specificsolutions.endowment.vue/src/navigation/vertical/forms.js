export default [
  { heading: 'Forms & Tables' },
  {
    title: 'Form Elements',
    icon: { icon: 'tabler-checkbox' },
    children: [
      { title: 'Autocomplete', to: 'forms-autocomplete', action: 'View', subject: 'otherview' },
      { title: 'Checkbox', to: 'forms-checkbox', action: 'View', subject: 'otherview' },
      { title: 'Combobox', to: 'forms-combobox', action: 'View', subject: 'otherview' },
      { title: 'Date Time Picker', to: 'forms-date-time-picker', action: 'View', subject: 'otherview' },
      { title: 'Editors', to: 'forms-editors', action: 'View', subject: 'otherview' },
      { title: 'File Input', to: 'forms-file-input', action: 'View', subject: 'otherview' },
      { title: 'Radio', to: 'forms-radio', action: 'View', subject: 'otherview' },
      { title: 'Custom Input', to: 'forms-custom-input', action: 'View', subject: 'otherview' },
      { title: 'Range Slider', to: 'forms-range-slider', action: 'View', subject: 'otherview' },
      { title: 'Rating', to: 'forms-rating', action: 'View', subject: 'otherview' },
      { title: 'Select', to: 'forms-select', action: 'View', subject: 'otherview' },
      { title: 'Slider', to: 'forms-slider', action: 'View', subject: 'otherview' },
      { title: 'Switch', to: 'forms-switch', action: 'View', subject: 'otherview' },
      { title: 'Textarea', to: 'forms-textarea', action: 'View', subject: 'otherview' },
      { title: 'Textfield', to: 'forms-textfield', action: 'View', subject: 'otherview' },
    ],
  },
  {
    title: 'Form Layouts',
    icon: { icon: 'tabler-layout' },
    to: 'forms-form-layouts',
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Form Wizard',
    icon: { icon: 'tabler-git-merge' },
    children: [
      { title: 'Numbered', to: 'forms-form-wizard-numbered', action: 'View', subject: 'otherview' },
      { title: 'Icons', to: 'forms-form-wizard-icons', action: 'View', subject: 'otherview' },
    ],
  },
  {
    title: 'Form Validation',
    icon: { icon: 'tabler-checkup-list' },
    to: 'forms-form-validation',
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Tables',
    icon: { icon: 'tabler-table' },
    children: [
      { title: 'Simple Table', to: 'tables-simple-table', action: 'View', subject: 'otherview' },
      { title: 'Data Table', to: 'tables-data-table', action: 'View', subject: 'otherview' },
    ],
  },
]
